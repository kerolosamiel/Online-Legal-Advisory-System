using ELawyer.Models.ViewModels.Admin.Client;
using ELawyer.Services.Admin;
using ELawyer.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELawyer.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.AdminRole)]
[Route("admin")]
public class ClientManagementsController : Controller
{
    private readonly IClientService _clientService;
    private readonly IExcelExportService _excelExportService;

    public ClientManagementsController(IClientService clientService, IExcelExportService excelExportService)
    {
        _clientService = clientService;
        _excelExportService = excelExportService;
    }

    [Route("clients")]
    [HttpGet]
    public async Task<IActionResult> Index(ClientFilter filter, int page = 1, int pageSize = 10)
    {
        try
        {
            var result = await _clientService.GetClientsWithPagination(filter, page, pageSize);

            if (result.Clients.Count == 0) ViewBag.Message = "No clients found.";

            return View(result);
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var client = await _clientService.GetClientDetails(id);
            return View(client);
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var client = await _clientService.GetClientForEdit(id);
            return View(client);
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ClientEditVm model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);

        try
        {
            var result = await _clientService.UpdateClient(model);
            return result ? RedirectToAction(nameof(Index)) : NotFound();
        }
        catch
        {
            ModelState.AddModelError("", "Error updating client");
            return View(model);
        }
    }

    [HttpPost("toggle-status/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        try
        {
            var result = await _clientService.ToggleClientStatus(id);
            return result ? RedirectToAction("Index") : NotFound();
        }
        catch
        {
            return StatusCode(500, "Error updating status");
        }
    }

    [HttpGet("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var client = await _clientService.GetClientForDelete(id);
            return View(client);
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }


    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(ClientDetailsVm model)
    {
        try
        {
            await _clientService.DeleteClient(model);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return StatusCode(500, "Error deleting client");
        }
    }

    [HttpPost("export-excel")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExportToExcel(ClientFilter filter)
    {
        try
        {
            var clients = await _clientService.GetClientsForExport(filter);
            var stream = _excelExportService.ExportClients(clients);
            return File(stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Clients_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
        }
        catch
        {
            return StatusCode(500, "Error generating Excel file");
        }
    }
}