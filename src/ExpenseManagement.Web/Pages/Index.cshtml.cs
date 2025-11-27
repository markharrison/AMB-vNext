using ExpenseManagement.Web.Models;
using ExpenseManagement.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseManagement.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IExpenseApiService _apiService;

    public IndexModel(IExpenseApiService apiService)
    {
        _apiService = apiService;
    }

    public DashboardStats Stats { get; set; } = new();
    public IEnumerable<Expense> RecentExpenses { get; set; } = Enumerable.Empty<Expense>();

    public async Task OnGetAsync()
    {
        Stats = await _apiService.GetDashboardStatsAsync();
        RecentExpenses = (await _apiService.GetAllExpensesAsync()).Take(10);
    }
}
