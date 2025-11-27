using ExpenseManagement.Api.Models;

namespace ExpenseManagement.Api.Services;

public interface IExpenseDataService
{
    // Expenses
    Task<IEnumerable<Expense>> GetAllExpensesAsync();
    Task<IEnumerable<Expense>> GetExpensesByUserAsync(int userId);
    Task<IEnumerable<Expense>> GetPendingApprovalsAsync();
    Task<Expense?> GetExpenseByIdAsync(int expenseId);
    Task<Expense> CreateExpenseAsync(CreateExpenseRequest request);
    Task<Expense?> SubmitExpenseAsync(int expenseId);
    Task<Expense?> ApproveExpenseAsync(int expenseId, int reviewerId);
    Task<Expense?> RejectExpenseAsync(int expenseId, int reviewerId);
    Task<bool> DeleteExpenseAsync(int expenseId);

    // Categories
    Task<IEnumerable<ExpenseCategory>> GetCategoriesAsync();
    Task<ExpenseCategory?> GetCategoryByIdAsync(int categoryId);

    // Statuses
    Task<IEnumerable<ExpenseStatus>> GetStatusesAsync();

    // Users
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User?> GetUserByIdAsync(int userId);

    // Dashboard
    Task<DashboardStats> GetDashboardStatsAsync();
}
