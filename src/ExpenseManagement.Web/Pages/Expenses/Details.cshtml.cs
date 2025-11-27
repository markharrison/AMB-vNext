using ExpenseManagement.Web.Models;
using ExpenseManagement.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseManagement.Web.Pages.Expenses;

public class DetailsModel : PageModel
{
    private readonly IExpenseApiService _apiService;

    public DetailsModel(IExpenseApiService apiService)
    {
        _apiService = apiService;
    }

    public Expense? Expense { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        Expense = await _apiService.GetExpenseByIdAsync(Id);
        if (Expense == null)
        {
            return NotFound();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostSubmitAsync()
    {
        await _apiService.SubmitExpenseAsync(Id);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        await _apiService.DeleteExpenseAsync(Id);
        return RedirectToPage("/Expenses/Index");
    }
}
