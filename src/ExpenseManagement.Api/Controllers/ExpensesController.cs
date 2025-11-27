using ExpenseManagement.Api.Models;
using ExpenseManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseDataService _dataService;

    public ExpensesController(IExpenseDataService dataService)
    {
        _dataService = dataService;
    }

    /// <summary>
    /// Get all expenses
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Expense>>> GetAll()
    {
        var expenses = await _dataService.GetAllExpensesAsync();
        return Ok(expenses);
    }

    /// <summary>
    /// Get expenses by user ID
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Expense>>> GetByUser(int userId)
    {
        var expenses = await _dataService.GetExpensesByUserAsync(userId);
        return Ok(expenses);
    }

    /// <summary>
    /// Get pending approvals
    /// </summary>
    [HttpGet("pending")]
    public async Task<ActionResult<IEnumerable<Expense>>> GetPendingApprovals()
    {
        var expenses = await _dataService.GetPendingApprovalsAsync();
        return Ok(expenses);
    }

    /// <summary>
    /// Get expense by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Expense>> GetById(int id)
    {
        var expense = await _dataService.GetExpenseByIdAsync(id);
        if (expense == null)
            return NotFound();
        return Ok(expense);
    }

    /// <summary>
    /// Create a new expense
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Expense>> Create([FromBody] CreateExpenseRequest request)
    {
        var expense = await _dataService.CreateExpenseAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = expense.ExpenseId }, expense);
    }

    /// <summary>
    /// Submit a draft expense for approval
    /// </summary>
    [HttpPost("{id}/submit")]
    public async Task<ActionResult<Expense>> Submit(int id)
    {
        var expense = await _dataService.SubmitExpenseAsync(id);
        if (expense == null)
            return NotFound("Expense not found or not in draft status");
        return Ok(expense);
    }

    /// <summary>
    /// Approve a submitted expense
    /// </summary>
    [HttpPost("{id}/approve")]
    public async Task<ActionResult<Expense>> Approve(int id, [FromBody] ApproveRejectRequest request)
    {
        var expense = await _dataService.ApproveExpenseAsync(id, request.ReviewerId);
        if (expense == null)
            return NotFound("Expense not found or not in submitted status");
        return Ok(expense);
    }

    /// <summary>
    /// Reject a submitted expense
    /// </summary>
    [HttpPost("{id}/reject")]
    public async Task<ActionResult<Expense>> Reject(int id, [FromBody] ApproveRejectRequest request)
    {
        var expense = await _dataService.RejectExpenseAsync(id, request.ReviewerId);
        if (expense == null)
            return NotFound("Expense not found or not in submitted status");
        return Ok(expense);
    }

    /// <summary>
    /// Delete an expense
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _dataService.DeleteExpenseAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
