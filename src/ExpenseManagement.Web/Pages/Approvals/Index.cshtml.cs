using ExpenseManagement.Web.Models;
using ExpenseManagement.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseManagement.Web.Pages.Approvals;

public class IndexModel : PageModel
{
    private readonly IExpenseApiService _apiService;

    public IndexModel(IExpenseApiService apiService)
    {
        _apiService = apiService;
    }

    public IEnumerable<Expense> PendingExpenses { get; set; } = Enumerable.Empty<Expense>();

    [BindProperty(SupportsGet = true)]
    public string? Filter { get; set; }

    public async Task OnGetAsync()
    {
        var allPending = await _apiService.GetPendingApprovalsAsync();

        if (!string.IsNullOrWhiteSpace(Filter))
        {
            PendingExpenses = allPending.Where(e =>
                (e.Description?.Contains(Filter, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (e.CategoryName?.Contains(Filter, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (e.UserName?.Contains(Filter, StringComparison.OrdinalIgnoreCase) ?? false)
            );
        }
        else
        {
            PendingExpenses = allPending;
        }
    }

    public async Task<IActionResult> OnPostApproveAsync(int expenseId)
    {
        await _apiService.ApproveExpenseAsync(expenseId, 2); // Bob Manager as reviewer
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRejectAsync(int expenseId)
    {
        await _apiService.RejectExpenseAsync(expenseId, 2); // Bob Manager as reviewer
        return RedirectToPage();
    }
}
