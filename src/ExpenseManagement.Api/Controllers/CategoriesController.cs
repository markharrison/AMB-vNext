using ExpenseManagement.Api.Models;
using ExpenseManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IExpenseDataService _dataService;

    public CategoriesController(IExpenseDataService dataService)
    {
        _dataService = dataService;
    }

    /// <summary>
    /// Get all expense categories
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExpenseCategory>>> GetAll()
    {
        var categories = await _dataService.GetCategoriesAsync();
        return Ok(categories);
    }

    /// <summary>
    /// Get category by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ExpenseCategory>> GetById(int id)
    {
        var category = await _dataService.GetCategoryByIdAsync(id);
        if (category == null)
            return NotFound();
        return Ok(category);
    }
}
