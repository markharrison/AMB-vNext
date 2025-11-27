using ExpenseManagement.Api.Models;

namespace ExpenseManagement.Api.Services;

public class InMemoryExpenseDataService : IExpenseDataService
{
    private readonly List<Role> _roles;
    private readonly List<User> _users;
    private readonly List<ExpenseCategory> _categories;
    private readonly List<ExpenseStatus> _statuses;
    private readonly List<Expense> _expenses;
    private int _nextExpenseId = 5;

    public InMemoryExpenseDataService()
    {
        // Initialize roles
        _roles = new List<Role>
        {
            new() { RoleId = 1, RoleName = "Employee", Description = "Regular employee who can submit expenses" },
            new() { RoleId = 2, RoleName = "Manager", Description = "Can view and approve/reject submitted expenses" }
        };

        // Initialize users
        _users = new List<User>
        {
            new() { UserId = 1, UserName = "Alice Example", Email = "alice@example.co.uk", RoleId = 1, ManagerId = 2, IsActive = true, RoleName = "Employee", ManagerName = "Bob Manager" },
            new() { UserId = 2, UserName = "Bob Manager", Email = "bob.manager@example.co.uk", RoleId = 2, ManagerId = null, IsActive = true, RoleName = "Manager", ManagerName = null }
        };

        // Initialize categories
        _categories = new List<ExpenseCategory>
        {
            new() { CategoryId = 1, CategoryName = "Travel", IsActive = true },
            new() { CategoryId = 2, CategoryName = "Meals", IsActive = true },
            new() { CategoryId = 3, CategoryName = "Supplies", IsActive = true },
            new() { CategoryId = 4, CategoryName = "Accommodation", IsActive = true },
            new() { CategoryId = 5, CategoryName = "Other", IsActive = true }
        };

        // Initialize statuses
        _statuses = new List<ExpenseStatus>
        {
            new() { StatusId = 1, StatusName = "Draft" },
            new() { StatusId = 2, StatusName = "Submitted" },
            new() { StatusId = 3, StatusName = "Approved" },
            new() { StatusId = 4, StatusName = "Rejected" }
        };

        // Initialize sample expenses
        _expenses = new List<Expense>
        {
            new()
            {
                ExpenseId = 1,
                UserId = 1,
                CategoryId = 1,
                StatusId = 2,
                AmountMinor = 2540,
                Currency = "GBP",
                ExpenseDate = new DateOnly(2025, 10, 20),
                Description = "Taxi from airport to client site",
                ReceiptFile = "/receipts/alice/taxi_oct20.jpg",
                SubmittedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UserName = "Alice Example",
                CategoryName = "Travel",
                StatusName = "Submitted"
            },
            new()
            {
                ExpenseId = 2,
                UserId = 1,
                CategoryId = 2,
                StatusId = 3,
                AmountMinor = 1425,
                Currency = "GBP",
                ExpenseDate = new DateOnly(2025, 9, 15),
                Description = "Client lunch meeting",
                ReceiptFile = "/receipts/alice/lunch_sep15.jpg",
                SubmittedAt = new DateTime(2025, 9, 16, 10, 15, 0),
                ReviewedBy = 2,
                ReviewedAt = new DateTime(2025, 9, 17, 14, 30, 0),
                CreatedAt = new DateTime(2025, 9, 15, 18, 30, 0),
                UserName = "Alice Example",
                CategoryName = "Meals",
                StatusName = "Approved",
                ReviewerName = "Bob Manager"
            },
            new()
            {
                ExpenseId = 3,
                UserId = 1,
                CategoryId = 3,
                StatusId = 1,
                AmountMinor = 799,
                Currency = "GBP",
                ExpenseDate = new DateOnly(2025, 11, 1),
                Description = "Office stationery",
                CreatedAt = DateTime.UtcNow,
                UserName = "Alice Example",
                CategoryName = "Supplies",
                StatusName = "Draft"
            },
            new()
            {
                ExpenseId = 4,
                UserId = 1,
                CategoryId = 4,
                StatusId = 3,
                AmountMinor = 12300,
                Currency = "GBP",
                ExpenseDate = new DateOnly(2025, 8, 10),
                Description = "Hotel during client visit",
                ReceiptFile = "/receipts/alice/hotel_aug10.jpg",
                SubmittedAt = new DateTime(2025, 8, 11, 9, 0, 0),
                ReviewedBy = 2,
                ReviewedAt = new DateTime(2025, 8, 12, 14, 30, 0),
                CreatedAt = new DateTime(2025, 8, 10, 20, 0, 0),
                UserName = "Alice Example",
                CategoryName = "Accommodation",
                StatusName = "Approved",
                ReviewerName = "Bob Manager"
            }
        };
    }

    public Task<IEnumerable<Expense>> GetAllExpensesAsync()
    {
        return Task.FromResult(_expenses.OrderByDescending(e => e.ExpenseDate).AsEnumerable());
    }

    public Task<IEnumerable<Expense>> GetExpensesByUserAsync(int userId)
    {
        return Task.FromResult(_expenses.Where(e => e.UserId == userId).OrderByDescending(e => e.ExpenseDate).AsEnumerable());
    }

    public Task<IEnumerable<Expense>> GetPendingApprovalsAsync()
    {
        return Task.FromResult(_expenses.Where(e => e.StatusId == 2).OrderBy(e => e.SubmittedAt).AsEnumerable());
    }

    public Task<Expense?> GetExpenseByIdAsync(int expenseId)
    {
        return Task.FromResult(_expenses.FirstOrDefault(e => e.ExpenseId == expenseId));
    }

    public Task<Expense> CreateExpenseAsync(CreateExpenseRequest request)
    {
        var user = _users.FirstOrDefault(u => u.UserId == request.UserId);
        var category = _categories.FirstOrDefault(c => c.CategoryId == request.CategoryId);
        var status = request.Submit ? _statuses.First(s => s.StatusId == 2) : _statuses.First(s => s.StatusId == 1);

        var expense = new Expense
        {
            ExpenseId = _nextExpenseId++,
            UserId = request.UserId,
            CategoryId = request.CategoryId,
            StatusId = status.StatusId,
            AmountMinor = (int)(request.Amount * 100),
            Currency = "GBP",
            ExpenseDate = request.ExpenseDate,
            Description = request.Description,
            ReceiptFile = request.ReceiptFile,
            SubmittedAt = request.Submit ? DateTime.UtcNow : null,
            CreatedAt = DateTime.UtcNow,
            UserName = user?.UserName,
            CategoryName = category?.CategoryName,
            StatusName = status.StatusName
        };

        _expenses.Add(expense);
        return Task.FromResult(expense);
    }

    public Task<Expense?> SubmitExpenseAsync(int expenseId)
    {
        var expense = _expenses.FirstOrDefault(e => e.ExpenseId == expenseId);
        if (expense == null || expense.StatusId != 1)
            return Task.FromResult<Expense?>(null);

        expense.StatusId = 2;
        expense.StatusName = "Submitted";
        expense.SubmittedAt = DateTime.UtcNow;
        return Task.FromResult<Expense?>(expense);
    }

    public Task<Expense?> ApproveExpenseAsync(int expenseId, int reviewerId)
    {
        var expense = _expenses.FirstOrDefault(e => e.ExpenseId == expenseId);
        if (expense == null || expense.StatusId != 2)
            return Task.FromResult<Expense?>(null);

        var reviewer = _users.FirstOrDefault(u => u.UserId == reviewerId);
        expense.StatusId = 3;
        expense.StatusName = "Approved";
        expense.ReviewedBy = reviewerId;
        expense.ReviewedAt = DateTime.UtcNow;
        expense.ReviewerName = reviewer?.UserName;
        return Task.FromResult<Expense?>(expense);
    }

    public Task<Expense?> RejectExpenseAsync(int expenseId, int reviewerId)
    {
        var expense = _expenses.FirstOrDefault(e => e.ExpenseId == expenseId);
        if (expense == null || expense.StatusId != 2)
            return Task.FromResult<Expense?>(null);

        var reviewer = _users.FirstOrDefault(u => u.UserId == reviewerId);
        expense.StatusId = 4;
        expense.StatusName = "Rejected";
        expense.ReviewedBy = reviewerId;
        expense.ReviewedAt = DateTime.UtcNow;
        expense.ReviewerName = reviewer?.UserName;
        return Task.FromResult<Expense?>(expense);
    }

    public Task<bool> DeleteExpenseAsync(int expenseId)
    {
        var expense = _expenses.FirstOrDefault(e => e.ExpenseId == expenseId);
        if (expense == null)
            return Task.FromResult(false);

        _expenses.Remove(expense);
        return Task.FromResult(true);
    }

    public Task<IEnumerable<ExpenseCategory>> GetCategoriesAsync()
    {
        return Task.FromResult(_categories.Where(c => c.IsActive).AsEnumerable());
    }

    public Task<ExpenseCategory?> GetCategoryByIdAsync(int categoryId)
    {
        return Task.FromResult(_categories.FirstOrDefault(c => c.CategoryId == categoryId));
    }

    public Task<IEnumerable<ExpenseStatus>> GetStatusesAsync()
    {
        return Task.FromResult(_statuses.AsEnumerable());
    }

    public Task<IEnumerable<User>> GetUsersAsync()
    {
        return Task.FromResult(_users.Where(u => u.IsActive).AsEnumerable());
    }

    public Task<User?> GetUserByIdAsync(int userId)
    {
        return Task.FromResult(_users.FirstOrDefault(u => u.UserId == userId));
    }

    public Task<DashboardStats> GetDashboardStatsAsync()
    {
        var stats = new DashboardStats
        {
            TotalExpenses = _expenses.Count,
            PendingApprovals = _expenses.Count(e => e.StatusId == 2),
            ApprovedAmount = _expenses.Where(e => e.StatusId == 3).Sum(e => e.Amount),
            ApprovedCount = _expenses.Count(e => e.StatusId == 3)
        };
        return Task.FromResult(stats);
    }
}
