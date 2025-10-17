# 🛒 E-Commerce Clean Architecture

A comprehensive e-commerce application built with **Clean Architecture** principles using **.NET 9** and **Entity Framework Core**.

[![.NET](https://img.shields.io/badge/.NET-9.0-purple)](https://dotnet.microsoft.com/)
[![EF Core](https://img.shields.io/badge/EF%20Core-9.0-blue)](https://docs.microsoft.com/en-us/ef/core/)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)

## 📋 Table of Contents

- [About The Project](#about-the-project)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Features](#features)
- [Database Schema](#database-schema)
- [Design Patterns](#design-patterns)
- [Learning Objectives](#learning-objectives)
- [Roadmap](#roadmap)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## 🎯 About The Project

This project is a **learning-focused implementation** of Clean Architecture in a real-world e-commerce scenario. It demonstrates best practices in:

- **Clean Architecture** layering and dependency inversion
- **Domain-Driven Design (DDD)** principles
- **Repository & Unit of Work** patterns
- **SOLID** principles
- **Entity Framework Core** advanced features
- **Separation of Concerns** across layers

### Why Clean Architecture?

Clean Architecture provides:
- ✅ **Independence** from frameworks, UI, and databases
- ✅ **Testability** through clear separation of concerns
- ✅ **Maintainability** with well-organized code structure
- ✅ **Flexibility** to change technology stack easily

## 🏗️ Architecture

The project follows the **Clean Architecture** pattern with four main layers:

```
┌─────────────────────────────────────────────────────┐
│                    API Layer                        │
│            (Controllers, Middleware)                │
└─────────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────────┐
│                Application Layer                    │
│     (Services, DTOs, Validators, Mapping)          │
└─────────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────────┐
│              Infrastructure Layer                   │
│  (DbContext, Repositories, Configurations)         │
└─────────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────────┐
│                  Domain Layer                       │
│        (Entities, Enums, Interfaces)               │
└─────────────────────────────────────────────────────┘
```

### Dependency Flow

- **API** → **Application** → **Infrastructure** → **Domain**
- **Domain** has **NO dependencies** on other layers (Framework-agnostic)
- **Application** depends only on **Domain**
- **Infrastructure** implements **Application** and **Domain** interfaces

## 🛠️ Tech Stack

### Core Technologies
- **.NET 9.0** - Latest .NET framework
- **C# 12** - Modern C# features
- **Entity Framework Core 9.0** - ORM for database access
- **SQL Server** - Database (LocalDB for development)

### Libraries & Packages
- **AutoMapper 12.0** - Object-to-object mapping
- **FluentValidation 11.9** - DTO validation
- **Microsoft.EntityFrameworkCore.SqlServer** - SQL Server provider
- **Microsoft.EntityFrameworkCore.Tools** - EF Core CLI tools

### Design Patterns
- Repository Pattern
- Unit of Work Pattern
- Generic Repository Pattern
- Dependency Injection
- Factory Pattern (Entity Configuration)

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) (or SQL Server)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) / [VS Code](https://code.visualstudio.com/) / [Rider](https://www.jetbrains.com/rider/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/YOUR-USERNAME/ECommerce-CleanArch.git
   cd ECommerce-CleanArch
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Update connection string** (optional)
   
   Edit `appsettings.json` in `ECommerence-CleanArch.API`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ECommerceCleanArchDB;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

4. **Apply database migrations**
   ```bash
   dotnet ef database update --project ECommerence-CleanArch.Infrastructure --startup-project ECommerence-CleanArch.API
   ```

5. **Run the application**
   ```bash
   dotnet run --project ECommerence-CleanArch.API
   ```

6. **Access Swagger UI** (when API controllers are implemented)
   ```
   https://localhost:5001/swagger
   ```

## 📁 Project Structure

```
ECommerence-CleanArch/
├── ECommerence-CleanArch.Domain/          # Core business logic (Framework-agnostic)
│   ├── Common/
│   │   ├── EntityBase.cs                  # Base entity with audit fields
│   │   └── Enums/                         # Domain enums
│   ├── Entity/                            # Domain entities
│   │   ├── Product.cs
│   │   ├── Category.cs
│   │   ├── Customer.cs
│   │   ├── Order.cs
│   │   ├── OrderItem.cs
│   │   ├── ShoppingCart.cs
│   │   └── CartItem.cs
│   └── Interfaces/                        # Repository interfaces (Legacy)
│
├── ECommerence-CleanArch.Application/     # Business rules & orchestration
│   ├── Common/                            # Shared application components
│   │   ├── IAsyncRepository.cs            # Generic repository interface
│   │   ├── EFAsyncRepositoryBase.cs       # Generic repository implementation
│   │   ├── IQuery.cs                      # Query interface
│   │   └── Result.cs                      # Result wrapper
│   ├── Contracts/                         # Application interfaces
│   │   ├── Repositories/                  # Specific repository interfaces
│   │   └── Services/                      # Service interfaces
│   ├── DTOs/                              # Data Transfer Objects
│   │   ├── Product/
│   │   ├── Category/
│   │   ├── Customer/
│   │   └── Order/
│   ├── Features/                          # Service implementations
│   │   ├── ProductService.cs
│   │   ├── CategoryService.cs
│   │   ├── CustomerService.cs
│   │   └── OrderService.cs
│   ├── Mapping/                           # AutoMapper profiles
│   │   └── MappingProfile.cs
│   ├── Validators/                        # FluentValidation validators
│   │   ├── Product/
│   │   ├── Category/
│   │   └── Customer/
│   ├── Paging/                            # Pagination helpers
│   │   └── Paginate.cs
│   └── ApplicationServiceRegistration.cs  # DI registration
│
├── ECommerence-CleanArch.Infrastructure/  # External concerns (DB, File, API)
│   ├── Persistance/
│   │   ├── Context/
│   │   │   └── ApplicationDbContext.cs    # EF Core DbContext
│   │   ├── Configurations/                # Entity configurations (Fluent API)
│   │   │   ├── ProductConfiguration.cs
│   │   │   ├── CategoryConfiguration.cs
│   │   │   ├── CustomerConfiguration.cs
│   │   │   ├── OrderConfiguration.cs
│   │   │   ├── OrderItemConfiguration.cs
│   │   │   ├── ShoppingCartConfiguration.cs
│   │   │   └── CartItemConfiguration.cs
│   │   └── Repositories/                  # Repository implementations (Legacy)
│   ├── Migrations/                        # EF Core migrations
│   └── DependencyInjection.cs             # Infrastructure DI registration
│
└── ECommerence-CleanArch.API/             # Presentation layer
    ├── Controllers/                       # API controllers (To be implemented)
    ├── Program.cs                         # Application entry point
    └── appsettings.json                   # Configuration

```

## ✨ Features

### Domain Layer
- ✅ **EntityBase** with audit fields (CreatedAt, UpdatedAt, DeletedAt, IsActive, IsDeleted)
- ✅ **7 Core Entities**: Product, Category, Customer, Order, OrderItem, ShoppingCart, CartItem
- ✅ **Enums**: OrderStatus, PaymentStatus, Currency
- ✅ **Navigation Properties** for relationships
- ✅ **Repository Interfaces** (Domain-driven contracts)

### Application Layer
- ✅ **Generic Repository Pattern** with `IAsyncRepository<T>`
- ✅ **Pagination Support** with `Paginate<T>`
- ✅ **DTOs** for all entities (Create, Update, Response)
- ✅ **AutoMapper** for entity-DTO mapping
- ✅ **FluentValidation** for DTO validation
- ✅ **Service Layer** with business logic
- ✅ **Result Wrapper** for standardized responses

### Infrastructure Layer
- ✅ **Entity Framework Core** with SQL Server
- ✅ **Fluent API** entity configurations
- ✅ **Global Query Filters** for soft delete
- ✅ **Audit Trail** automatic timestamp management
- ✅ **Repository Implementations** (Generic + Specific)
- ✅ **Unit of Work** pattern
- ✅ **Migrations** for database versioning

### Validation Rules

#### Product Validation
- Name: Required, Max 200 characters
- Description: Required, Max 1000 characters
- Price: Must be greater than 0
- Stock: Cannot be negative
- SKU: Required, Max 50 characters, Unique
- CategoryId: Required
- PriceCurrency: Valid enum value

#### Category Validation
- Name: Required, Max 100 characters
- Description: Max 500 characters

#### Customer Validation
- FirstName: Required, Max 100 characters
- LastName: Required, Max 100 characters
- Email: Required, Valid email format, Max 200 characters
- PhoneNumber: Required, Valid phone format, Max 20 characters
- Address: Required, Max 500 characters

## 🗄️ Database Schema

### Core Entities

**Products**
- Stores product information with pricing, stock, and SKU
- Supports multiple currencies (USD, EUR, GBP, TRY)
- Links to Categories via foreign key

**Categories**
- Hierarchical structure (Parent-Child relationships)
- One-to-many with Products
- Self-referencing for subcategories

**Customers**
- User information and contact details
- One-to-many with Orders
- One-to-many with ShoppingCarts

**Orders**
- Order tracking with status (Pending, Processing, Shipped, Delivered, Cancelled)
- Payment tracking (Pending, Completed, Failed, Refunded)
- Links to Customer and OrderItems

**OrderItems**
- Individual items in an order
- Stores quantity and price at order time
- Links to Order and Product

**ShoppingCarts**
- Active shopping sessions
- Links to Customer and CartItems

**CartItems**
- Items in shopping cart
- Stores quantity
- Links to ShoppingCart and Product

### Soft Delete Support
All entities support soft delete via:
- `IsDeleted` flag
- `DeletedAt` timestamp
- Global query filter (automatically excludes deleted records)

## 🎨 Design Patterns

### 1. Repository Pattern
Abstracts data access logic and provides a collection-like interface for domain entities.

```csharp
public interface IAsyncRepository<TEntity> where TEntity : EntityBase
{
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<Paginate<TEntity>> GetListAsync(...);
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false);
}
```

### 2. Unit of Work Pattern
Coordinates the work of multiple repositories and maintains a list of objects affected by a business transaction.

```csharp
public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    ICategoryRepository Categories { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
```

### 3. Generic Repository Pattern
Provides common CRUD operations for all entities, reducing code duplication.

### 4. Dependency Injection
All dependencies are injected through constructors, following the Dependency Inversion Principle.

### 5. Factory Pattern
Entity configurations are separated into factory classes using `IEntityTypeConfiguration<T>`.

## 📚 Learning Objectives

This project was built to learn and demonstrate:

### Clean Architecture Principles
- ✅ Dependency Inversion (Domain has no dependencies)
- ✅ Separation of Concerns (Each layer has clear responsibility)
- ✅ Framework Independence (Domain is framework-agnostic)
- ✅ Testability (Easy to unit test)

### SOLID Principles
- ✅ **S**ingle Responsibility (One class, one purpose)
- ✅ **O**pen/Closed (Open for extension, closed for modification)
- ✅ **L**iskov Substitution (Derived classes can substitute base classes)
- ✅ **I**nterface Segregation (Many specific interfaces over one general)
- ✅ **D**ependency Inversion (Depend on abstractions, not concretions)

### Entity Framework Core
- ✅ DbContext configuration
- ✅ Fluent API entity mapping
- ✅ Global query filters
- ✅ Migrations
- ✅ Navigation properties
- ✅ Eager loading (`.Include()`)
- ✅ Tracking vs. No-tracking queries

### Domain-Driven Design
- ✅ Entities and value objects
- ✅ Aggregate roots
- ✅ Repository pattern
- ✅ Domain interfaces
- ✅ Domain enums

### Advanced C# Features
- ✅ Nullable reference types
- ✅ Expression trees
- ✅ Generic constraints
- ✅ Extension methods
- ✅ Primary constructors (.NET 9)
- ✅ Record types (DTOs)

## 🗺️ Roadmap

### Phase 1: Core Architecture ✅ (Completed)
- [x] Domain layer setup
- [x] Infrastructure layer with EF Core
- [x] Application layer with services
- [x] Repository pattern implementation
- [x] Generic repository pattern
- [x] AutoMapper integration
- [x] FluentValidation integration

### Phase 2: API Layer 🚧 (In Progress)
- [ ] REST API controllers
- [ ] Swagger/OpenAPI documentation
- [ ] Error handling middleware
- [ ] Request/Response logging
- [ ] API versioning

### Phase 3: Advanced Features 📅 (Planned)
- [ ] CQRS pattern with MediatR
- [ ] Authentication & Authorization (JWT)
- [ ] Role-based access control
- [ ] Caching (Redis)
- [ ] Background jobs (Hangfire)
- [ ] Email notifications
- [ ] File upload service

### Phase 4: Testing 📅 (Planned)
- [ ] Unit tests (xUnit)
- [ ] Integration tests
- [ ] Repository tests
- [ ] Service tests
- [ ] Test coverage reports

### Phase 5: DevOps 📅 (Future)
- [ ] Docker containerization
- [ ] CI/CD pipeline (GitHub Actions)
- [ ] Health checks
- [ ] Monitoring & logging (Serilog)
- [ ] Performance optimization

## 🤝 Contributing

Contributions are welcome! This is a learning project, and I'm open to suggestions and improvements.

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

Distributed under the MIT License. See `LICENSE` for more information.

## 📧 Contact

Your Name - [@dugerdev](https://twitter.com/your_twitter) - muhammedduger@outlook.com

Project Link: [https://github.com/dugerdev/ECommerce-CleanArch](https://github.com/dugerdev/ECommerce-CleanArch)

## 🙏 Acknowledgments

- [Clean Architecture - Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design - Eric Evans](https://www.domainlanguage.com/ddd/)
- [Microsoft .NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)

---

⭐ **If you found this project helpful, please consider giving it a star!** ⭐

