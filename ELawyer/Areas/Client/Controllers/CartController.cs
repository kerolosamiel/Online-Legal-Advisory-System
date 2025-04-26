using ELawyer.DataAccess.Data;
using ELawyer.DataAccess.Repository;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;
using ELawyer.Models.ViewModels;
using ELawyer.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.IO;
using System.Security.Claims;

namespace ELawyer.Areas.Client.Controllers
{
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        public IWebHostEnvironment _webHostEnvironment { get; }
        public CartController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {

            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
        }

        public IActionResult AddToCart(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            var Client = _unitOfWork.Client.Get(l => l.ID == user.ClientID);


            var Lawyer = _unitOfWork.Lawyer.Get(l => l.ID == id, includeproperties: "specializationnews.Specialization,specializationnews.Specialization.SubSpecializations,Service");
            var Rating = _unitOfWork.Rating.GetAll(r => r.lawyerID == id, includeproperties: "Client");
            var ratings = _unitOfWork.Rating.GetAll(r => r.lawyerID == id);

            Payment payment = new Payment();
            payment.Amount = (double)Lawyer.ConsultationFee + ((double)Lawyer.ConsultationFee * .15);
            payment.lawyerID = Lawyer.ID;
            payment.ClientID = Client.ID;


            if (ratings == null)
            {
                Lawyer.AverageRateing = 0;
            }
            else
            {
                Lawyer.AverageRateing = ratings.Average(r => r.Rate);
            }
            ClientLawyerRating clientLawyerRating = new ClientLawyerRating()
            {
                Lawyer = Lawyer,
                Rating = Rating,
                Payment = payment



            };

            _unitOfWork.Payment.Add(payment);
            _unitOfWork.Save();

            return View(clientLawyerRating);
        }

        public IActionResult Cartitem()
        {


            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            var Client = _unitOfWork.Client.Get(l => l.ID == user.ClientID);

            var payment = _unitOfWork.Payment.Get(p => p.ClientID == Client.ID && p.PaidAt == null, includeproperties: "Lawyer,Client");
            if (payment != null)
            {

                return View(payment);
            }



            return View("ShowLawyers");



        }

        public IActionResult DeleteCartitem()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            var Client = _unitOfWork.Client.Get(l => l.ID == user.ClientID);


            var payment = _unitOfWork.Payment.Get(p => p.ClientID == Client.ID && p.PaidAt == null);
            _unitOfWork.Payment.Remove(payment);
            _unitOfWork.Save();


            return RedirectToAction("Index", "Lawyer");
        }


        public IActionResult PaymentStripe(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var domain = Request.Scheme + "://" + Request.Host.Value + "/";
            //Note:
            //Create Api Session Stripe ===> Take this Code from (Session Api Referances) Site

            var payment = _unitOfWork.Payment.Get(Payment => Payment.Id == id, includeproperties: "Lawyer,Client");
            var lawyer = _unitOfWork.Lawyer.Get(l => l.ID == payment.lawyerID, includeproperties: "Service");
            var options = new SessionCreateOptions
            {
                //Note:
                //when we click (pay) that will take us to this link
                SuccessUrl = domain + $"Cart/Consultation?id={payment.Id}",
                CancelUrl = domain + "Lawyer/Index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            //Note:
            //we will papulate this by our self

            var sessionLineItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(payment.Amount * 100), // 20.50 => 2050
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = lawyer.Service.Title
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
            Session session = service.Create(options);
           
            _unitOfWork.Payment.Update(payment);
            _unitOfWork.Save();



            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            var Client = _unitOfWork.Client.Get(l => l.ID == user.ClientID);
           
            
            string emailBody = $@"
    <p>مرحبًا {Client.FirstName},</p>
    <p>لقد تم إتمام الدفع بنجاح.</p>
    <p><strong>تفاصيل الدفع:</strong></p>
    <ul>
        <li><strong>المبلغ المدفوع:</strong> {payment.Amount} جنيه</li>
        <li><strong>تاريخ الدفع:</strong> {payment.PaidAt}</li>
        <li><strong>اسم المحامي:</strong> {payment.Lawyer.FirstName} {payment.Lawyer.LastName}</li>
    </ul>
    <p>شكرًا لاختيارك خدمة ELawyer.</p>
";

            _emailSender.SendEmailAsync(user.Email, "Payment- ELawyer", emailBody);


            //Note:
            //save the respone in this url
            Response.Headers.Add("Location", session.Url);
            //Note:
            //return the url
            return new StatusCodeResult(303);


        }


        public IActionResult Consultation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = _unitOfWork.Payment.Get(Payment => Payment.Id == id, includeproperties: "Lawyer,Client");
            var consultation = new ConsultationPaymentViewModel()
            {
                Payment = payment,
            };
            return View(consultation);
        }

        [HttpPost]
        public async Task<IActionResult> ConsultationPost(ConsultationPaymentViewModel consultationPayment, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                Consultation consultation1 = new Consultation();


                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); 
                    string lawyerPdfPath = Path.Combine(wwwRootPath, @"files\Lawyer\Attachments");

                    
                    if (!Directory.Exists(lawyerPdfPath))
                        Directory.CreateDirectory(lawyerPdfPath);

                    
                    using (var fileStream = new FileStream(Path.Combine(lawyerPdfPath, fileName), FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
                    {
                        await file.CopyToAsync(fileStream); 
                    }

                   
                    consultation1.Attachments = @"files\Lawyer\Attachments" + fileName;
                }

                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
                var Client = _unitOfWork.Client.Get(l => l.ID == user.ClientID);
                var lawyer = _unitOfWork.Lawyer.Get(Lawyer => Lawyer.ID == consultationPayment.Payment.lawyerID);

                consultation1.Title = consultationPayment.Consultation.Title;
                consultation1.Description = consultationPayment.Consultation.Description;
                consultation1.lawyerID = lawyer.ID;
                consultation1.ClientID = Client.ID;

                consultation1.PaymentId = consultationPayment.Payment.Id;
                _unitOfWork.Consultation.Add(consultation1);
                _unitOfWork.Save();

                var user2 = _unitOfWork.ApplicationUser.Get(u => u.LawyerID == lawyer.ID);

                _emailSender.SendEmailAsync(user2.Email, "New Consultation- ELawyer", "<P> You Have new Consultation</P>");



            }
            return RedirectToAction("Index", "Lawyer");
        }

        public IActionResult ShowClienConsultation()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            var Lawyer = _unitOfWork.Lawyer.Get(l => l.ID == user.LawyerID);
            var consultation = _unitOfWork.Consultation.GetAll(c => c.lawyerID == Lawyer.ID, includeproperties: "Client");
         
         

                return View(consultation);
        }


        public IActionResult ResponseLawyer(int? id)
        {
            Consultation consultation = _unitOfWork.Consultation.Get(c => c.ID == id, includeproperties: "Client");
            Response response = new Response()
            {
                ConsultationID = consultation.ID,
                ClientID = consultation.ClientID
            };
            ResponseConsultationViewModel responseConsultation = new ResponseConsultationViewModel()
            {
                Consultation = consultation,
                Response = response
            };

            return View(responseConsultation);
        }
        [HttpPost]
        public async Task<IActionResult> ResponseLawyerPost(ResponseConsultationViewModel responseConsultation, IFormFile file)
        {

            if (ModelState.IsValid)
            {
                Response response = new Response();


                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); 
                    string lawyerPdfPath = Path.Combine(wwwRootPath, @"files\Lawyer\Attachments");

                    
                    if (!Directory.Exists(lawyerPdfPath))
                        Directory.CreateDirectory(lawyerPdfPath);

                  

                    
                    using (var fileStream = new FileStream(Path.Combine(lawyerPdfPath, fileName), FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
                    {
                        await file.CopyToAsync(fileStream); 
                    }

                    
                    response.Attachments = @"files\Lawyer\Attachments" + fileName;
                }

                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
                var lawyer = _unitOfWork.Lawyer.Get(l => l.ID == user.LawyerID);
                var client = _unitOfWork.Client.Get(Lawyer => Lawyer.ID == responseConsultation.Consultation.ClientID);

                response.Title = responseConsultation.Response.Title;
                response.Description = responseConsultation.Response.Description;
                response.lawyerID = lawyer.ID;
                response.ClientID = client.ID;

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
                var consultation = _unitOfWork.Consultation.Get(c => c.ClientID == client.ID && c.lawyerID == lawyer.ID);

                if (!string.IsNullOrEmpty(consultation.Attachments))
                {
                    var oldFilePath = Path.Combine(wwwRootPath, consultation.Attachments.TrimStart('\\'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                _unitOfWork.Consultation.Remove(consultation);
                _unitOfWork.Save();
                var payment = _unitOfWork.Payment.Get(p => p.ClientID == client.ID && p.Recievedat == null && p.lawyerID == lawyer.ID);

                _unitOfWork.Payment.Update(payment);
                _unitOfWork.Save();

                var user2 = _unitOfWork.ApplicationUser.Get(u => u.ClientID == client.ID);
                _emailSender.SendEmailAsync(user2.Email, "Payment- ELawyer", "<p> Alawyer has been respond<p/>");



            }
            return RedirectToAction("Index", "Lawyer");
        }



        public IActionResult ShowLawyerResponse()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            var Client = _unitOfWork.Client.Get(l => l.ID == user.ClientID);
            var response = _unitOfWork.Response.GetAll(c => c.ClientID == Client.ID, includeproperties: "Lawyer");

            return View(response);
        }


        public IActionResult Download(string fileName)
        {
            fileName = fileName.Replace("Attachments", "Attachments\\");
           
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            
            string filePath = Path.Combine(wwwRootPath, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

          
            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            
            return File(fileBytes, "application/pdf", fileName);
        }


    }
}
