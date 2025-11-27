using ExpenseManagement.Web.Models;
using ExpenseManagement.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ExpenseManagement.Web.Pages.Expenses;

public class CreateModel : PageModel
{
    private readonly IExpenseApiService _apiService;

    public CreateModel(IExpenseApiService apiService)
    {
        _apiService = apiService;
    }

    [BindProperty]
    public ExpenseInputModel Input { get; set; } = new();

    public SelectList? Categories { get; set; }

    public class ExpenseInputModel
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateOnly ExpenseDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Description")]
        [StringLength(1000)]
        public string? Description { get; set; }

        [Display(Name = "Submit for approval")]
        public bool Submit { get; set; }
    }

    public async Task OnGetAsync()
    {
        await LoadCategoriesAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync();
            return Page();
        }

        var request = new CreateExpenseRequest
        {
            UserId = 1, // Default to Alice for demo
            CategoryId = Input.CategoryId,
            Amount = Input.Amount,
            ExpenseDate = Input.ExpenseDate,
            Description = Input.Description,
            Submit = Input.Submit
        };

        await _apiService.CreateExpenseAsync(request);
        return RedirectToPage("/Expenses/Index");
    }

    private async Task LoadCategoriesAsync()
    {
        var categories = await _apiService.GetCategoriesAsync();
        Categories = new SelectList(categories, "CategoryId", "CategoryName");
    }
}
