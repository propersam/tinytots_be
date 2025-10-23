# Tinytots E-Commerce API

A comprehensive ASP.NET Core 8.0 Web API for managing a children's product e-commerce platform. This API handles product catalogs, categories, orders, and invoicing for a kids' clothing and accessories store.

## Table of Contents

- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [IDE-Specific Setup](#ide-specific-setup)
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

## IDE-Specific Setup

This section provides detailed setup instructions for different IDEs. Choose the section that matches your preferred development environment.

### Visual Studio 2022 Setup

Visual Studio is Microsoft's full-featured IDE with excellent .NET support and integrated tools.

1. **Download and Install Visual Studio 2022**
   - Visit [Visual Studio Downloads](https://visualstudio.microsoft.com/downloads/)
   - Download "Visual Studio 2022 Community" (free version)
   - During installation, select the ".NET desktop development" workload
   - Also select "ASP.NET and web development" workload

2. **Open the Project**
   - Launch Visual Studio 2022
   - Click "Open a project or solution"
   - Navigate to the `tinytots_be` folder and select `Tinytots.csproj`
   - Alternatively, open the solution file `tinytots.sln` from the root directory

3. **Restore NuGet Packages**
   - Right-click on the project in Solution Explorer
   - Select "Restore NuGet Packages"
   - Or use the menu: Tools → NuGet Package Manager → Restore NuGet Packages

4. **Configure Database Connection**
   - In Solution Explorer, find `appsettings.Development.json`
   - Right-click and select "Open"
   - Update the connection string with your MySQL credentials

5. **Run Database Migrations**
   - Open Package Manager Console: View → Other Windows → Package Manager Console
   - Run: `Update-Database`

6. **Run the Application**
   - Press F5 or click the green "Run" button
   - The application will start and open your default browser to Swagger UI

**Tips:**
- Use the Solution Explorer to navigate project files
- Press Ctrl+K, Ctrl+D to format code
- Use F12 to go to definition of methods/classes
- Enable IntelliSense by typing and using Ctrl+Space

### Rider Setup

Rider is JetBrains' cross-platform IDE with powerful refactoring tools and excellent .NET support.

1. **Download and Install Rider**
   - Visit [JetBrains Rider](https://www.jetbrains.com/rider/)
   - Download and install Rider (free 30-day trial, or use student license)
   - Install .NET 8.0 SDK if not already installed

2. **Open the Project**
   - Launch Rider
   - Click "Open" and select the `tinytots_be` folder
   - Rider will automatically detect the .NET project

3. **Restore Dependencies**
   - Rider will prompt to restore NuGet packages - click "Restore"
   - Or use: Tools → NuGet → Restore NuGet Packages

4. **Configure Database Connection**
   - In the Project view, find `appsettings.Development.json`
   - Open and update the MySQL connection string

5. **Run Database Migrations**
   - Open Terminal: View → Tool Windows → Terminal
   - Run: `dotnet ef database update`

6. **Run the Application**
   - Click the green "Run" button in the toolbar
   - Or press Shift+F10
   - Swagger UI will open automatically

**Tips:**
- Use Ctrl+N to quickly find classes
- Press Ctrl+B to go to declaration
- Use Alt+Enter for quick fixes and refactoring suggestions
- Enable code completion by typing and using Ctrl+Space

### Visual Studio Code Setup

VS Code is a lightweight, extensible editor that's great for .NET development with the right extensions.

1. **Download and Install VS Code**
   - Visit [VS Code Downloads](https://code.visualstudio.com/download)
   - Install VS Code for your operating system

2. **Install Required Extensions**
   - Open VS Code
   - Click the Extensions icon (square icon on left sidebar)
   - Install these extensions:
     - "C#" by Microsoft (provides IntelliSense, debugging, etc.)
     - "C# Dev Kit" by Microsoft (enhanced .NET development)
     - ".NET MAUI" by Microsoft (optional, for mobile development)
     - "NuGet Package Manager" by jmrog
     - "EditorConfig for VS Code" by EditorConfig

3. **Open the Project Folder**
   - File → Open Folder
   - Select the `tinytots_be` folder
   - VS Code will detect the C# project and prompt to add required assets

4. **Restore Dependencies**
   - Open integrated terminal: View → Terminal
   - Run: `dotnet restore`

5. **Configure Database Connection**
   - Open `appsettings.Development.json`
   - Update the MySQL connection string with your credentials

6. **Run Database Migrations**
   - In terminal: `dotnet ef database update`

7. **Run the Application**
   - In terminal: `dotnet run`
   - Or press F5 to debug
   - Open browser to `http://localhost:5215` for Swagger

**Tips:**
- Use Ctrl+Shift+P to open command palette
- Press F12 to go to definition
- Use Ctrl+. for quick actions and refactoring
- Install "Prettier" extension for code formatting
- Use integrated terminal for running commands

### Common Setup Steps for All IDEs

After completing IDE-specific setup:

1. **Verify Database Connection**
   - Check that MySQL is running
   - Test connection by running the app and checking logs

2. **Test the API**
   - Open Swagger UI at `http://localhost:5215`
   - Try the GET endpoints first (Categories, Products, etc.)
   - Create test data using POST endpoints

3. **Debugging Tips**
   - Set breakpoints by clicking in the left margin of code
   - Use F5 to start debugging
   - Watch variables in the debug panel
   - Check console output for errors

4. **Version Control**
   - Use Git integration in your IDE
   - Commit changes regularly
   - Pull latest changes before starting work

## Database Setup

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

### Code Formatting

This project uses **dotnet-format** with automated code formatting on every build. The formatting rules are defined in `.editorconfig`.

**Automatic Formatting:**
- Code is automatically formatted when you run `dotnet build`
- Format is enforced before compilation starts
- Ensures consistent code style across the entire project

**Manual Formatting Commands:**

```bash
# Format all code files
dotnet format

# Check if files need formatting (without making changes)
dotnet format --verify-no-changes

# Format with detailed output
dotnet format --verbosity diagnostic
```

**Formatting Rules (from .editorconfig):**

1. **Indentation**: 4 spaces for C# files
2. **Naming Conventions**:
   - Interfaces: Start with `I` (e.g., `IProductService`)
   - Classes/Methods: PascalCase (e.g., `ProductController`)
   - Private fields: Start with underscore (e.g., `_context`)
3. **Braces**: Always use braces, even for single-line blocks
4. **var keyword**: Use for built-in types only when type is apparent
5. **Using directives**: System namespaces first, alphabetically sorted
6. **Whitespace**: Specific rules for spacing around operators and keywords

**IDE Integration:**

Most IDEs will automatically pick up the `.editorconfig` file:
- **Visual Studio**: Automatic
- **VS Code**: Install "EditorConfig for VS Code" extension
- **Rider**: Automatic

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
