using ExpenseManagement.Web.Models;
using ExpenseManagement.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseManagement.Web.Pages.Expenses;

public class IndexModel : PageModel
{
    private readonly IExpenseApiService _apiService;

    public IndexModel(IExpenseApiService apiService)
    {
        _apiService = apiService;
    }

    public IEnumerable<Expense> Expenses { get; set; } = Enumerable.Empty<Expense>();
    
    [BindProperty(SupportsGet = true)]
    public string? Filter { get; set; }

    public async Task OnGetAsync()
    {
        var allExpenses = await _apiService.GetAllExpensesAsync();
        
        if (!string.IsNullOrWhiteSpace(Filter))
        {
            Expenses = allExpenses.Where(e => 
                (e.Description?.Contains(Filter, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (e.CategoryName?.Contains(Filter, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (e.StatusName?.Contains(Filter, StringComparison.OrdinalIgnoreCase) ?? false)
            );
        }
        else
        {
            Expenses = allExpenses;
        }
    }

    public async Task<IActionResult> OnPostSubmitAsync(int id)
    {
        await _apiService.SubmitExpenseAsync(id);
        return RedirectToPage();
    }
}
