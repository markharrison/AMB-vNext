using ExpenseManagement.Api.Models;
using ExpenseManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IExpenseDataService _dataService;

    public DashboardController(IExpenseDataService dataService)
    {
        _dataService = dataService;
    }

    /// <summary>
    /// Get dashboard statistics
    /// </summary>
    [HttpGet("stats")]
    public async Task<ActionResult<DashboardStats>> GetStats()
    {
        var stats = await _dataService.GetDashboardStatsAsync();
        return Ok(stats);
    }
}
