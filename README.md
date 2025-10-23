# Tinytots E-Commerce API

A comprehensive ASP.NET Core 8.0 Web API for managing a children's product e-commerce platform. This API handles product catalogs, categories, orders, and invoicing for a kids' clothing and accessories store.

## Table of Contents

- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Database Setup](#database-setup)
- [API Endpoints](#api-endpoints)
- [Architecture](#architecture)
- [Development Guide](#development-guide)
- [Testing](#testing)
- [Troubleshooting](#troubleshooting)

## Overview

Tinytots is a RESTful API designed for managing an e-commerce platform specializing in children's products. The system supports:

- **Product Management**: CRUD operations for products with categories and subcategories
- **Category Organization**: Hierarchical category system (Category → SubCategory → Products)
- **Order Processing**: Create and manage customer orders
- **Invoice Generation**: Automated invoice creation with unique tracking codes

## Tech Stack

- **Framework**: ASP.NET Core 8.0
- **Language**: C# 12
- **Database**: MySQL 9.4
- **ORM**: Entity Framework Core 8.0.19
- **Database Provider**: Pomelo.EntityFrameworkCore.MySql 8.0.3
- **API Documentation**: Swagger/OpenAPI (Swashbuckle 6.6.2)

## Project Structure

```
tinytots_be/
├── Controllers/          # API endpoint controllers
│   ├── CategoryController.cs
│   ├── SubCategoryController.cs
│   ├── ProductController.cs
│   ├── OrderController.cs
│   └── InvoiceController.cs
├── Models/              # Database entity models
│   ├── Category.cs
│   ├── SubCategory.cs
│   ├── Product.cs
│   ├── Order.cs
│   └── Invoice.cs
├── DTO/                 # Data Transfer Objects
│   ├── CategoryDTO.cs
│   ├── SubCategoryDTO.cs
│   ├── ProductDTO.cs
│   ├── OrderDTO.cs
│   └── InvoiceDTO.cs
├── DbContext/           # Database context configuration
│   └── TinytotsDbContext.cs
├── Enums/               # Application enumerations
│   ├── StatusEnum.cs
│   ├── GenderEnum.cs
│   └── AgeGroupEnum.cs
├── Services/            # Business logic services
│   └── OrderService.cs
├── Interfaces/          # Service interfaces
│   ├── ICategoryService.cs
│   ├── IProductService.cs
│   └── IOrderService.cs
├── Migrations/          # EF Core database migrations
└── Program.cs           # Application entry point
```

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL Server 9.4+](https://dev.mysql.com/downloads/mysql/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) with C# extension

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd tinytots_be
   ```

2. **Install dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure Database Connection**

   Update `appsettings.Development.json` with your MySQL credentials:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "server=localhost;port=3306;database=tinytots;Username=YOUR_USERNAME;Password=YOUR_PASSWORD"
     }
   }
   ```

4. **Apply Database Migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the Application**
   ```bash
   dotnet run
   ```

6. **Access Swagger Documentation**

   Navigate to: `http://localhost:5215/`

## Database Setup

### Initial Migration

If you need to create migrations from scratch:

```bash
# Remove old migrations
rm -rf Migrations/

# Create new migration
dotnet ef migrations add InitialCreate

# Apply to database
dotnet ef database update
```

### Database Schema

**Categories Table**
- `CategoryId` (PK, INT, Auto-increment)
- `Name` (VARCHAR(20), Required)

**SubCategories Table**
- `SubCategoryId` (PK, INT, Auto-increment)
- `Name` (VARCHAR(15), Required)
- `CategoryId` (FK to Categories, INT)

**Products Table**
- `ProductId` (PK, INT, Auto-increment)
- `Name` (VARCHAR(25), Required)
- `SubCategoryId` (FK to SubCategories, INT)
- `Quantity` (INT, Required)
- `UnitPrice` (DECIMAL(10,2), Required)
- `CreatedAt` (DATETIME(6))

**Orders Table**
- `OrderId` (PK, INT, Auto-increment)
- `OrderCode` (VARCHAR, Unique)
- `Name` (VARCHAR(20))
- `ProductId` (FK to Products, INT)
- `Quantity` (INT)
- `UnitPrice` (DECIMAL(10,2))
- `LinePrice` (DECIMAL(20,2))
- `InvoiceId` (FK to Invoices, INT, Nullable)
- `CreatedAt` (DATETIME(6))

**Invoices Table**
- `InvId` (PK, INT, Auto-increment)
- `InvCode` (VARCHAR(20), Unique)
- `InvoiceBill` (DECIMAL(20,2))
- `Status` (ENUM: Pending, Paid, Dispatched, Delivered)

## API Endpoints

All endpoints follow RESTful conventions and return JSON responses.

### Categories

| Method | Endpoint | Description | Request Body |
|--------|----------|-------------|--------------|
| GET | `/api/Category` | Get all categories | - |
| GET | `/api/Category/{id}` | Get category by ID | - |
| POST | `/api/Category` | Create new category | `{ "Name": "string" }` |
| PATCH | `/api/Category/{id}` | Update category | `{ "Name": "string" }` |
| DELETE | `/api/Category/{id}` | Delete category | - |

### SubCategories

| Method | Endpoint | Description | Request Body |
|--------|----------|-------------|--------------|
| GET | `/api/SubCategory` | Get all subcategories | - |
| GET | `/api/SubCategory/{id}` | Get subcategory by ID | - |
| POST | `/api/SubCategory` | Create new subcategory | `{ "Name": "string", "CategoryName": "string" }` |
| PATCH | `/api/SubCategory/{id}` | Update subcategory | `{ "Name": "string" }` |
| DELETE | `/api/SubCategory/{id}` | Delete subcategory | - |

### Products

| Method | Endpoint | Description | Request Body |
|--------|----------|-------------|--------------|
| GET | `/api/Product` | Get all products | - |
| GET | `/api/Product/{id}` | Get product by ID | - |
| POST | `/api/Product` | Create new product | `{ "Name": "string", "Price": 0, "Quantity": 0, "SubCategoryName": "string" }` |
| PATCH | `/api/Product/{id}` | Update product | `{ "Name": "string", "Price": 0, "Quantity": 0, "SubCategoryName": "string" }` |
| DELETE | `/api/Product/{id}` | Delete product | - |

### Orders

| Method | Endpoint | Description | Request Body |
|--------|----------|-------------|--------------|
| GET | `/api/Order` | Get all orders | - |
| GET | `/api/Order/{id}` | Get order by ID | - |
| POST | `/api/Order` | Create new order | `{ "ProductId": 0, "Quantity": 0 }` |
| PATCH | `/api/Order/{id}` | Update order quantity | `{ "Quantity": 0 }` |
| DELETE | `/api/Order/{id}` | Delete order | - |

### Invoices

| Method | Endpoint | Description | Request Body |
|--------|----------|-------------|--------------|
| GET | `/api/Invoice` | Get all invoices | - |
| GET | `/api/Invoice/{id}` | Get invoice by ID | - |
| POST | `/api/Invoice` | Create new invoice | `{ "Items": [{ "ProductId": 0, "Quantity": 0 }] }` |
| PATCH | `/api/Invoice/{id}/status` | Update invoice status | `0-3` (0=Pending, 1=Paid, 2=Dispatched, 3=Delivered) |
| DELETE | `/api/Invoice/{id}` | Delete invoice | - |

## Architecture

### Design Patterns

1. **Repository Pattern**: Each controller interacts with the database through `DbContext`
2. **DTO Pattern**: Data Transfer Objects separate API contracts from database models
3. **Dependency Injection**: Services and DbContext are injected into controllers

### Data Flow

```
Client Request
    ↓
API Controller (Validation)
    ↓
Business Logic
    ↓
Entity Framework Core
    ↓
MySQL Database
    ↓
Response (JSON)
```

### Key Concepts

**Entity Relationships**
```
Category (1) ─── (Many) SubCategory
                           │
                           └─── (Many) Product
                                        │
                                        └─── (Many) Order
                                                     │
                                                     └─── (Many) Invoice
```

**Order Code Generation**
- Format: `TTORD-YYYYMMDD-XXXXX`
- Example: `TTORD-20251023-45678`

**Invoice Code Generation**
- Format: `TTINV-YYYYMMDD-XXXXX`
- Example: `TTINV-20251023-12345`

## Development Guide

### Adding a New Endpoint

1. **Create DTO** (if needed)
   ```csharp
   public class NewFeatureDTO
   {
       public required string Name { get; set; }
       public int Value { get; set; }
   }
   ```

2. **Add Controller Action**
   ```csharp
   [HttpPost]
   public async Task<IActionResult> CreateFeature(NewFeatureDTO dto)
   {
       // Validate
       if (!ModelState.IsValid)
           return BadRequest(ModelState);

       // Process
       // ...

       // Return
       return CreatedAtAction(nameof(GetFeature),
           new { id = feature.Id },
           feature);
   }
   ```

3. **Update Documentation** in this README

### Database Migrations

**Create a new migration:**
```bash
dotnet ef migrations add DescriptiveName
```

**Apply migration:**
```bash
dotnet ef database update
```

**Rollback last migration:**
```bash
dotnet ef migrations remove
```

**Reset database (CAUTION: Deletes all data):**
```bash
dotnet ef database drop --force
dotnet ef database update
```

### Code Style Guidelines

- Use **async/await** for all database operations
- Always include **try-catch** blocks in controller actions
- Return appropriate **HTTP status codes**:
  - 200 OK - Successful GET/PATCH
  - 201 Created - Successful POST
  - 204 No Content - Successful DELETE
  - 400 Bad Request - Validation errors
  - 404 Not Found - Resource not found
  - 409 Conflict - Duplicate resource
  - 500 Internal Server Error - Unexpected errors

## Testing

### Manual Testing with Swagger

1. Run the application: `dotnet run`
2. Navigate to: `http://localhost:5215/`
3. Use Swagger UI to test endpoints interactively

### Testing Workflow Example

1. **Create a Category**
   ```json
   POST /api/Category
   { "Name": "Boys" }
   ```

2. **Create a SubCategory**
   ```json
   POST /api/SubCategory
   { "Name": "T-Shirts", "CategoryName": "Boys" }
   ```

3. **Create a Product**
   ```json
   POST /api/Product
   {
     "Name": "Blue T-Shirt",
     "Price": 25.99,
     "Quantity": 50,
     "SubCategoryName": "T-Shirts"
   }
   ```

4. **Create an Order**
   ```json
   POST /api/Order
   { "ProductId": 1, "Quantity": 3 }
   ```

5. **Create an Invoice**
   ```json
   POST /api/Invoice
   {
     "Items": [
       { "ProductId": 1, "Quantity": 3 }
     ]
   }
   ```

### Using cURL

```bash
# Get all products
curl http://localhost:5215/api/Product

# Create a category
curl -X POST http://localhost:5215/api/Category \
  -H "Content-Type: application/json" \
  -d '{"Name": "Girls"}'

# Update invoice status to Paid (1)
curl -X PATCH http://localhost:5215/api/Invoice/1/status \
  -H "Content-Type: application/json" \
  -d '1'
```

## Troubleshooting

### Common Issues

**1. Database Connection Errors**
```
Solution: Verify MySQL is running and credentials in appsettings.Development.json are correct
```

**2. Migration Errors**
```bash
# Clean and recreate migrations
dotnet ef database drop --force
rm -rf Migrations/
dotnet ef migrations add InitialCreate
dotnet ef database update
```

**3. Build Errors**
```bash
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

**4. Port Already in Use**
```bash
# Change port in launchSettings.json or appsettings.json
# Or kill the process using port 5215
```

### Logs and Debugging

- **Console Logs**: Check terminal for `Console.WriteLine()` output
- **EF Core Logs**: Set logging level in `appsettings.Development.json`:
  ```json
  {
    "Logging": {
      "LogLevel": {
        "Microsoft.EntityFrameworkCore.Database.Command": "Information"
      }
    }
  }
  ```

## Contributing

1. Create a feature branch: `git checkout -b feature/your-feature`
2. Make your changes
3. Test thoroughly
4. Commit: `git commit -m "Add: your feature description"`
5. Push: `git push origin feature/your-feature`
6. Create a Pull Request

## Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [REST API Best Practices](https://restfulapi.net/)
- [MySQL Documentation](https://dev.mysql.com/doc/)

## License

This project is licensed under the MIT License - see the LICENSE file for details.

---

**Need Help?** Check the [Troubleshooting](#troubleshooting) section or contact the development team.
