# Expense Management System

A modern web application built with ASP.NET Core Razor Pages (.NET 10) that modernizes a legacy expense management system.

## Architecture

The solution consists of two projects:

- **ExpenseManagement.Web** - Razor Pages frontend application
- **ExpenseManagement.Api** - RESTful API backend

## Features

Based on the legacy application screenshots, this modern implementation includes:

- **Dashboard** - Overview with statistics (Total Expenses, Pending Approvals, Approved Amount, Approved Count)
- **Expenses List** - View all expenses with filtering capability
- **New Expense** - Form to add new expenses with Amount, Date, Category, and Description
- **Approvals** - Manager view to approve or reject submitted expenses
- **API Documentation** - OpenAPI endpoint at `/openapi/v1.json`

## Technology Stack

- .NET 10 (LTS)
- ASP.NET Core Razor Pages
- ASP.NET Core Web API
- In-memory data storage (development mode)
- SQL Server support (production mode)
- Bootstrap 5 for UI styling

## Running Locally

### Prerequisites

- .NET 10 SDK

### Start the API

```bash
cd src/ExpenseManagement.Api
dotnet run --urls "http://localhost:5001"
```

### Start the Web Application

In a separate terminal:

```bash
cd src/ExpenseManagement.Web
dotnet run --urls "http://localhost:5000"
```

### Access the Application

- Web Application: http://localhost:5000
- API Health Check: http://localhost:5001/api/health
- API OpenAPI Document: http://localhost:5001/openapi/v1.json

## API Endpoints

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/expenses` | GET | Get all expenses |
| `/api/expenses/{id}` | GET | Get expense by ID |
| `/api/expenses` | POST | Create new expense |
| `/api/expenses/{id}/submit` | POST | Submit expense for approval |
| `/api/expenses/{id}/approve` | POST | Approve expense |
| `/api/expenses/{id}/reject` | POST | Reject expense |
| `/api/expenses/pending` | GET | Get pending approvals |
| `/api/categories` | GET | Get expense categories |
| `/api/users` | GET | Get users |
| `/api/dashboard/stats` | GET | Get dashboard statistics |
| `/api/health` | GET | Health check |

## Database Schema

The application uses the schema defined in `/Database-Schema/database_schema.sql`:

- **Roles** - User roles (Employee, Manager)
- **Users** - System users with manager relationships
- **ExpenseCategories** - Travel, Meals, Supplies, Accommodation, Other
- **ExpenseStatus** - Draft, Submitted, Approved, Rejected
- **Expenses** - Expense records with amounts in minor units (pence)

## Development vs Production

- **Development**: Uses in-memory data storage with sample data
- **Production**: Configure SQL Server connection string in `appsettings.json`
