using ExpenseManagement.Api.Models;
using ExpenseManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusesController : ControllerBase
{
    private readonly IExpenseDataService _dataService;

    public StatusesController(IExpenseDataService dataService)
    {
        _dataService = dataService;
    }

    /// <summary>
    /// Get all expense statuses
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExpenseStatus>>> GetAll()
    {
        var statuses = await _dataService.GetStatusesAsync();
        return Ok(statuses);
    }
}
