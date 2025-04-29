using System.Security.Claims;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;
using ELawyer.Models.ViewModels;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace ELawyer.Areas.Client.Controllers;

public class CartController : Controller
{
    private readonly IEmailSender _emailSender;
    private readonly IUnitOfWork _unitOfWork;

    public CartController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
    {
        _unitOfWork = unitOfWork;
        WebHostEnvironment = webHostEnvironment;
        _emailSender = emailSender;
    }

    public IWebHostEnvironment WebHostEnvironment { get; }

    public IActionResult AddToCart(int id)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity!;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
        var client = _unitOfWork.Client.Get(l => l.Id == user.ClientId);


        var lawyer = _unitOfWork.Lawyer.Get(l => l.Id == id,
            "specializations.Specialization,specializations.Specialization.SubSpecializations,Service");
        var rating = _unitOfWork.Rating.GetAll(r => r.LawyerId == id, "Client");
        var ratings = _unitOfWork.Rating.GetAll(r => r.LawyerId == id);

        Payment payment = new();
        payment.Amount = (double)lawyer.ConsultationFee! + (double)lawyer.ConsultationFee * .15;
        payment.LawyerId = lawyer.Id;
        payment.ClientId = client.Id;


        lawyer.AverageRateing = ratings?.Average(r => r.Rate) ?? 0;

        var clientLawyerRating = new ClientLawyerRating
        {
            Lawyer = lawyer,
            Rating = rating,
            Payment = payment
        };

        _unitOfWork.Payment.Add(payment);
        _unitOfWork.Save();

        return View(clientLawyerRating);
    }

    public IActionResult Cartitem()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity!;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
        var client = _unitOfWork.Client.Get(l => l.Id == user.ClientId);

        var payment = _unitOfWork.Payment.Get(p => p.ClientId == client.Id && p.PaidAt == null, "Lawyer,Client");

        if (payment != null)
            return View(payment);

        return View("ShowLawyers");
    }

    public IActionResult DeleteCartitem()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity!;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
        var client = _unitOfWork.Client.Get(l => l.Id == user.ClientId);


        var payment = _unitOfWork.Payment.Get(p => p.ClientId == client.Id && p.PaidAt == null);
        _unitOfWork.Payment.Remove(payment);
        _unitOfWork.Save();

        return RedirectToAction("Index", "Lawyer");
    }


    public IActionResult PaymentStripe(int? paymentId, int? serviceId)
    {
        if (paymentId == null || paymentId == null) return NotFound();

        var domain = Request.Scheme + "://" + Request.Host.Value + "/";
        //Note:
        //Create Api Session Stripe ===> Take this Code from (Session Api Referances) Site

        var payment = _unitOfWork.Payment.Get(p => p.Id == paymentId, "Lawyer,Client");
        var lawyer = _unitOfWork.Lawyer.Get(l => l.Id == payment.LawyerId, "Services");

        // Find the specific service
        var selectedService = lawyer.Services.FirstOrDefault(s => s.Id == serviceId);

        if (selectedService == null)
            return BadRequest("Selected service not found for this lawyer.");

        var options = new SessionCreateOptions
        {
            //Note:
            //when we click (pay) that will take us to this link
            SuccessUrl = domain + $"Cart/Consultation?id={payment.Id}",
            CancelUrl = domain + "Lawyer/Index",
            LineItems = new List<SessionLineItemOptions>(),
            Mode = "payment"
        };

        //Note:
        //we will populate this by our self

        var sessionLineItem = new SessionLineItemOptions
        {
            PriceData = new SessionLineItemPriceDataOptions
            {
                UnitAmount = (long)(payment.Amount * 100), // 20.50 => 2050
                Currency = "usd",
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = selectedService.Title
                }
            },
            Quantity = 1
        };

        //Note:
        //Add session to line item
        options.LineItems.Add(sessionLineItem);

        var service = new SessionService();
        //Note:
        //create session
        var session = service.Create(options);

        _unitOfWork.Payment.Update(payment);
        _unitOfWork.Save();


        _unitOfWork.serviceOrder.Add(new ServiceOrder
        {
            PaymentId = payment.Id,
            ServiceId = selectedService.Id,
            CreatedAt = DateTime.Now,
            ScheduledAt = DateTime.Now.AddDays(2),
            ClientId = payment.ClientId,
            Amount = payment.Amount
        });
        _unitOfWork.Save();


        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
        var client = _unitOfWork.Client.Get(l => l.Id == user.ClientId);

        var serviceOrder = _unitOfWork.serviceOrder.Get(s => s.PaymentId == payment.Id);
        _unitOfWork.invoice.Add(new Invoice
        {
            Amount = payment.Amount,
            ServiceOrderId = serviceOrder.Id,
            PaymentDate = DateTime.Now,
            PaymentId = payment.Id,
            ClientId = client.Id
        });

        _unitOfWork.Save();

        var emailBody = $@"
    <p>مرحبًا {client.ApplicationUser.FirstName},</p>
    <p>لقد تم إتمام الدفع بنجاح.</p>
    <p><strong>تفاصيل الدفع:</strong></p>
    <ul>
        <li><strong>المبلغ المدفوع:</strong> {payment.Amount} جنيه</li>
        <li><strong>تاريخ الدفع:</strong> {payment.PaidAt}</li>
        <li><strong>اسم المحامي:</strong> {payment.Lawyer?.ApplicationUser.FirstName ?? "NA"} {payment.Lawyer?.ApplicationUser.LastName ?? "NA"}</li>
    </ul>
    <p>شكرًا لاختيارك خدمة ELawyer.</p>
";

        _emailSender.SendEmailAsync(user.Email ?? "NA", "Payment- ELawyer", emailBody);


        //Note:
        //save the respone in this url
        Response.Headers.Add("Location", session.Url);
        //Note:
        //return the url
        return new StatusCodeResult(303);
    }


    public IActionResult Consultation(int? id)
    {
        if (id == null) return NotFound();

        var payment = _unitOfWork.Payment.Get(Payment => Payment.Id == id, "Lawyer,Client");
        var consultation = new ConsultationPaymentViewModel
        {
            Payment = payment
        };
        return View(consultation);
    }

    [HttpPost]
    public async Task<IActionResult> ConsultationPost(ConsultationPaymentViewModel consultationPayment, IFormFile file)
    {
        if (ModelState.IsValid)
        {
            var consultation1 = new Consultation();


            var wwwRootPath = WebHostEnvironment.WebRootPath;
            if (file != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var lawyerPdfPath = Path.Combine(wwwRootPath, @"files\Lawyer\Attachments");


                if (!Directory.Exists(lawyerPdfPath))
                    Directory.CreateDirectory(lawyerPdfPath);


                using (var fileStream = new FileStream(Path.Combine(lawyerPdfPath, fileName), FileMode.Create,
                           FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
                {
                    await file.CopyToAsync(fileStream);
                }


                consultation1.Attachments = @"files\Lawyer\Attachments" + fileName;
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            var Client = _unitOfWork.Client.Get(l => l.Id == user.ClientId);
            var lawyer = _unitOfWork.Lawyer.Get(Lawyer => Lawyer.Id == consultationPayment.Payment.LawyerId);

            consultation1.Title = consultationPayment.Consultation.Title;
            consultation1.Description = consultationPayment.Consultation.Description;
            consultation1.LawyerId = lawyer.Id;
            consultation1.ClientId = Client.Id;

            consultation1.PaymentId = consultationPayment.Payment.Id;
            _unitOfWork.Consultation.Add(consultation1);
            _unitOfWork.Save();

            var user2 = _unitOfWork.ApplicationUser.Get(u => u.LawyerId == lawyer.Id);

            _emailSender.SendEmailAsync(user2.Email, "New Consultation- ELawyer", "<P> You Have new Consultation</P>");
        }

        return RedirectToAction("Index", "Lawyer");
    }

    public IActionResult ShowClienConsultation()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
        var Lawyer = _unitOfWork.Lawyer.Get(l => l.Id == user.LawyerId);
        var consultation = _unitOfWork.Consultation.GetAll(c => c.LawyerId == Lawyer.Id, "Client");


        return View(consultation);
    }


    public IActionResult ResponseLawyer(int? id)
    {
        var consultation = _unitOfWork.Consultation.Get(c => c.Id == id, "Client");
        var response = new Response
        {
            ConsultationId = consultation.Id,
            ClientId = consultation.ClientId
        };
        var responseConsultation = new ResponseConsultationViewModel
        {
            Consultation = consultation,
            Response = response
        };

        return View(responseConsultation);
    }

    [HttpPost]
    public async Task<IActionResult> ResponseLawyerPost(ResponseConsultationViewModel responseConsultation,
        IFormFile file)
    {
        if (ModelState.IsValid)
        {
            var response = new Response();


            var wwwRootPath = WebHostEnvironment.WebRootPath;
            if (file != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var lawyerPdfPath = Path.Combine(wwwRootPath, @"files\Lawyer\Attachments");


                if (!Directory.Exists(lawyerPdfPath))
                    Directory.CreateDirectory(lawyerPdfPath);


                using (var fileStream = new FileStream(Path.Combine(lawyerPdfPath, fileName), FileMode.Create,
                           FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
                {
                    await file.CopyToAsync(fileStream);
                }


                response.Attachments = @"files\Lawyer\Attachments" + fileName;
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            var lawyer = _unitOfWork.Lawyer.Get(l => l.Id == user.LawyerId);
            var client = _unitOfWork.Client.Get(Lawyer => Lawyer.Id == responseConsultation.Consultation.ClientId);

            response.Title = responseConsultation.Response.Title;
            response.Description = responseConsultation.Response.Description;
            response.LawyerId = lawyer.Id;
            response.ClientId = client.Id;

            _unitOfWork.Response.Add(response);
            _unitOfWork.Save();
            if (lawyer.NoOfClients == null)
                lawyer.NoOfClients = 1;
            else
                lawyer.NoOfClients += 1;
            if (client.NoOfLawyers == null)
                client.NoOfLawyers = 1;
            else
                client.NoOfLawyers += 1;
            _unitOfWork.Client.Update(client);
            _unitOfWork.Lawyer.Update(lawyer);
            _unitOfWork.Save();
            var consultation = _unitOfWork.Consultation.Get(c => c.ClientId == client.Id && c.LawyerId == lawyer.Id);

            if (!string.IsNullOrEmpty(consultation.Attachments))
            {
                var oldFilePath = Path.Combine(wwwRootPath, consultation.Attachments.TrimStart('\\'));
                if (System.IO.File.Exists(oldFilePath)) System.IO.File.Delete(oldFilePath);
            }

            _unitOfWork.Consultation.Remove(consultation);
            _unitOfWork.Save();
            var payment = _unitOfWork.Payment.Get(p =>
                p.ClientId == client.Id && p.Recievedat == null && p.LawyerId == lawyer.Id);

            _unitOfWork.Payment.Update(payment);
            _unitOfWork.Save();

            var user2 = _unitOfWork.ApplicationUser.Get(u => u.ClientId == client.Id);
            _emailSender.SendEmailAsync(user2.Email, "Payment- ELawyer", "<p> Alawyer has been respond<p/>");
        }

        return RedirectToAction("Index", "Lawyer");
    }


    public IActionResult ShowLawyerResponse()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
        var Client = _unitOfWork.Client.Get(l => l.Id == user.ClientId);
        var response = _unitOfWork.Response.GetAll(c => c.ClientId == Client.Id, "Lawyer");

        return View(response);
    }


    public IActionResult ShowInvoices()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
        var Client = _unitOfWork.Client.Get(l => l.Id == user.ClientId);
        var Invocies = _unitOfWork.invoice.GetAll(i => i.ClientId == Client.Id);
        return View(Invocies);
    }

    public IActionResult ShowServieOrders()
    {
        var servicesorders = _unitOfWork.serviceOrder.GetAll();
        return View(servicesorders);
    }


    public IActionResult Download(string fileName)
    {
        fileName = fileName.Replace("Attachments", "Attachments\\");

        var wwwRootPath = WebHostEnvironment.WebRootPath;


        var filePath = Path.Combine(wwwRootPath, fileName);

        if (!System.IO.File.Exists(filePath)) return NotFound();


        var fileBytes = System.IO.File.ReadAllBytes(filePath);


        return File(fileBytes, "application/pdf", fileName);
    }
}