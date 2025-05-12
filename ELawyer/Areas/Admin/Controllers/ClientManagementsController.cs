using ELawyer.Models.ViewModels.Admin.Client;
using ELawyer.Services.Admin;
using ELawyer.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELawyer.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.AdminRole)]
public class ClientManagementsController : Controller
{
    private readonly IClientService _clientService;
    private readonly IExcelExportService _excelExportService;

    public ClientManagementsController(IClientService clientService, IExcelExportService excelExportService)
    {
        _clientService = clientService;
        _excelExportService = excelExportService;
    }

    /*[Route("admin/clients")]*/
    [HttpGet]
    public async Task<IActionResult> Index(ClientFilter filter, int page = 1, int pageSize = 10)
    {
        try
        {
            var result = await _clientService.GetClientsWithPagination(filter, page, pageSize);

            if (result?.Clients == null || result.Clients.Count == 0) ViewBag.Message = "No clients found.";

            return View(result);
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var client = await _clientService.GetClientDetails(id);
            return client != null ? View(client) : NotFound();
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new ClientCreateVm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClientCreateVm model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            await _clientService.CreateClient(model);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            ModelState.AddModelError("", "Error creating client");
            return View(model);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var client = await _clientService.GetClientForEdit(id);
            return client != null ? View(client) : NotFound();
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("{id}")]
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

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        try
        {
            var result = await _clientService.ToggleClientStatus(id);
            return result ? RedirectToAction(nameof(Index)) : NotFound();
        }
        catch
        {
            return StatusCode(500, "Error updating status");
        }
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _clientService.DeleteClient(id);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return StatusCode(500, "Error deleting client");
        }
    }

    [HttpPost]
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