using ExpenseManagement.Web.Models;
using System.Text.Json;

namespace ExpenseManagement.Web.Services;

public interface IExpenseApiService
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

    // Statuses
    Task<IEnumerable<ExpenseStatus>> GetStatusesAsync();

    // Users
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User?> GetUserByIdAsync(int userId);

    // Dashboard
    Task<DashboardStats> GetDashboardStatsAsync();
}

public class ExpenseApiService : IExpenseApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ExpenseApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<IEnumerable<Expense>> GetAllExpensesAsync()
    {
        var response = await _httpClient.GetAsync("api/expenses");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<Expense>>(content, _jsonOptions) ?? Enumerable.Empty<Expense>();
    }

    public async Task<IEnumerable<Expense>> GetExpensesByUserAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"api/expenses/user/{userId}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<Expense>>(content, _jsonOptions) ?? Enumerable.Empty<Expense>();
    }

    public async Task<IEnumerable<Expense>> GetPendingApprovalsAsync()
    {
        var response = await _httpClient.GetAsync("api/expenses/pending");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<Expense>>(content, _jsonOptions) ?? Enumerable.Empty<Expense>();
    }

    public async Task<Expense?> GetExpenseByIdAsync(int expenseId)
    {
        var response = await _httpClient.GetAsync($"api/expenses/{expenseId}");
        if (!response.IsSuccessStatusCode)
            return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Expense>(content, _jsonOptions);
    }

    public async Task<Expense> CreateExpenseAsync(CreateExpenseRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/expenses", request);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Expense>(content, _jsonOptions)!;
    }

    public async Task<Expense?> SubmitExpenseAsync(int expenseId)
    {
        var response = await _httpClient.PostAsync($"api/expenses/{expenseId}/submit", null);
        if (!response.IsSuccessStatusCode)
            return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Expense>(content, _jsonOptions);
    }

    public async Task<Expense?> ApproveExpenseAsync(int expenseId, int reviewerId)
    {
        var request = new ApproveRejectRequest { ReviewerId = reviewerId };
        var response = await _httpClient.PostAsJsonAsync($"api/expenses/{expenseId}/approve", request);
        if (!response.IsSuccessStatusCode)
            return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Expense>(content, _jsonOptions);
    }

    public async Task<Expense?> RejectExpenseAsync(int expenseId, int reviewerId)
    {
        var request = new ApproveRejectRequest { ReviewerId = reviewerId };
        var response = await _httpClient.PostAsJsonAsync($"api/expenses/{expenseId}/reject", request);
        if (!response.IsSuccessStatusCode)
            return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Expense>(content, _jsonOptions);
    }

    public async Task<bool> DeleteExpenseAsync(int expenseId)
    {
        var response = await _httpClient.DeleteAsync($"api/expenses/{expenseId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<ExpenseCategory>> GetCategoriesAsync()
    {
        var response = await _httpClient.GetAsync("api/categories");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<ExpenseCategory>>(content, _jsonOptions) ?? Enumerable.Empty<ExpenseCategory>();
    }

    public async Task<IEnumerable<ExpenseStatus>> GetStatusesAsync()
    {
        var response = await _httpClient.GetAsync("api/statuses");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<ExpenseStatus>>(content, _jsonOptions) ?? Enumerable.Empty<ExpenseStatus>();
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        var response = await _httpClient.GetAsync("api/users");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<User>>(content, _jsonOptions) ?? Enumerable.Empty<User>();
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"api/users/{userId}");
        if (!response.IsSuccessStatusCode)
            return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<User>(content, _jsonOptions);
    }

    public async Task<DashboardStats> GetDashboardStatsAsync()
    {
        var response = await _httpClient.GetAsync("api/dashboard/stats");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DashboardStats>(content, _jsonOptions) ?? new DashboardStats();
    }
}
