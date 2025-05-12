using ELawyer.Models.ViewModels.Admin.Client;
using OfficeOpenXml;

namespace ELawyer.Services.Admin;

public class ExcelExportService : IExcelExportService
{
    public Stream ExportClients(List<ClientItemVm> clients)
    {
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Clients");
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Full Name";
            worksheet.Cells[1, 3].Value = "Email";
            worksheet.Cells[1, 4].Value = "Registration Date";
            worksheet.Cells[1, 5].Value = "Status";

            for (var i = 0; i < clients.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = clients[i].Id;
                worksheet.Cells[i + 2, 2].Value = clients[i].FullName;
                worksheet.Cells[i + 2, 3].Value = clients[i].Email;
                worksheet.Cells[i + 2, 4].Style.Numberformat.Format = "yyyy-mm-dd";
                worksheet.Cells[i + 2, 4].Value = clients[i].RegistrationDate;
                worksheet.Cells[i + 2, 5].Value = clients[i].Status;
            }

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;
            return stream;
        }
    }
}