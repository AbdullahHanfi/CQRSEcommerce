
# Shop API - Clean Architecture .NET 8 Solution

##  Project Summary

**Shop API** is a robust, scalable RESTful Web API built using **.NET 8** and **Clean Architecture** principles. This project serves as a comprehensive backend solution for an e-commerce platform, featuring secure authentication, product management, and cloud integration.

The solution demonstrates enterprise-level patterns including **CQRS** (Command Query Responsibility Segregation) using MediatR, **Repository & Unit of Work** patterns, **Decorator Pattern** for caching, and domain-driven design (DDD) core concepts.

## 🏗️ Architecture

The solution follows the **Clean Architecture** (also known as Onion Architecture) ensuring a separation of concerns and dependency inversion.

### Layers

1.  **Core (Domain & Application)**
    
    -   **Domain:** The heart of the software. Contains enterprise logic, Entities (`Product`, `User`), Enums, and Interface definitions (`IRepository`). No external dependencies.
        
    -   **Application:** Orchestrates business logic using **CQRS** with **MediatR**. Contains DTOs, Validators (`FluentValidation`), and Pipeline Behaviors (Logging, Validation, Exception Handling).
        
2.  **Infrastructure**
    
    -   **Persistence:** Implements data access using **EF Core** and **SQL Server**. Handles Migrations, Seeding, and the Unit of Work implementation.
        
    -   **Identity:** Manages Authentication and Authorization using **ASP.NET Core Identity** and **JWT** (JSON Web Tokens).
        
    -   **Shared:** Handles cross-cutting concerns such as **Redis Caching** (implemented via the Decorator pattern) and Cloud File Storage (S3/DigitalOcean Spaces).
        
3.  **Presentation**
    
    -   **WebAPI:** The entry point. Contains Controllers, Middleware (Global Exception Handling), and Swagger configuration.
        

## 📂 Project Structure

```
src/
├── Core/
│   ├── Application/           # Business Logic, CQRS, Interfaces, DTOs
│   │   ├── Features/          # Commands & Queries (Auth, Products)
│   │   ├── PipelineBehaviors/ # Cross-cutting concerns (Logging, Validation)
│   │   └── ...
│   └── Domain/                # Enterprise Entities, Repo Interfaces, Exceptions
│       ├── Entities/
│       └── Shared/
├── Infrastructure/
│   ├── Infrastructure.Identity/    # JWT & Identity Framework
│   ├── Infrastructure.Persistence/ # EF Core, DbContext, Migrations
│   └── Infrastructure.Shared/      # Redis, Cloud Storage, Services
└── Presentation/
    └── WebAPI/                # API Controllers, Program.cs, Middleware
```

##  Technologies & Tools

-   **Framework:** .NET 8
    
-   **Architecture:** Clean Architecture, CQRS (MediatR)
    
-   **Database:** SQL Server, Entity Framework Core 8
    
-   **Caching:** Redis (StackExchange.Redis)
    
-   **Validation:** FluentValidation
    
-   **Logging:** Serilog
    
-   **Storage:** Amazon S3 / DigitalOcean Spaces (AWSSDK.S3)
    
-   **Documentation:** Swagger / OpenAPI
    
-   **Authentication:** JWT Bearer, ASP.NET Core Identity
    

##  Core Features

###  Authentication & Security

-   **User Registration & Login:** Secure identity management.
    
-   **JWT Implementation:** Access Tokens and Refresh Tokens with rotation mechanisms.
    
-   **Token Revocation:** Capability to revoke tokens manually.
    

###  Product Domain

-   **Product Management:** Create, Read, and manage products.
    
-   **Image Handling:** Upload product images directly to S3-compatible cloud storage.
    
-   **Pagination:** Optimized `PaginatedList` implementation for fetching large datasets.
    
-   **Validation:** Strict server-side validation for all inputs.
    

###  Performance & Reliability

-   **Cached Repository:** Implements the Decorator pattern to transparently cache repository calls in Redis.
    
-   **Global Exception Handling:** Centralized middleware transforms exceptions into standardized JSON responses.
    
-   **Pipeline Behaviors:** Automatic request logging and validation before hitting handlers.
    

##  Setup & Configuration

### Prerequisites

1.  [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0 "null")
    
2.  [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads "null")
    
3.  [Redis](https://redis.io/ "null") (Local or Docker)
    

### 1. Clone the Repository

```
git clone https://github.com/AbdullahHanfi/CQRSEcommerce.git
```

### 2. appsettings.json Configuration

Navigate to `src/Presentation/WebAPI/appsettings.json` and configure your connection strings and keys:

```
{
  "ConnectionStrings": {
    "SQL": "Server=.;Database=ShopDb;Trusted_Connection=True;TrustServerCertificate=True;",
    "Redis": "localhost:6379"
  },
  "JWT": {
    "Key": "YOUR_SUPER_SECRET_LONG_KEY_MUST_BE_AT_LEAST_32_CHARS",
    "Issuer": "ShopApi",
    "Audience": "ShopClient",
    "DurationInDays": 1
  },
  "DigitalOceanSpace": {
    "AccessKey": "YOUR_ACCESS_KEY",
    "SecretKey": "YOUR_SECRET_KEY",
    "ServiceUrl": "https://nyc3.digitaloceanspaces.com",
    "BucketName": "your-bucket-name"
  }
}
```

### 3. Apply Migrations

Initialize the database and apply migrations:

```
cd src/Presentation/WebAPI
dotnet ef database update --project ../../Infrastructure/Infrastructure.Persistence
```

##  Build & Run

### Development Mode

To run the API locally:

```
cd src/Presentation/WebAPI
dotnet run
```

-   The API will be available at `https://localhost:5050` (or configured port).
    
-   **Swagger UI:** Navigate to `https://localhost:5050/swagger` to test endpoints interactively.
    

### Production Build

To publish the application for deployment:

```
dotnet publish -c Release -o ./publish
```
## 🌐 Deployment (In Progress) 

I am currently working on deploying the API to a production environment. The target infrastructure is a **DigitalOcean Droplet** configured with Nginx as a reverse proxy.

### Planned Hosting Stack

-   **Cloud Provider:** DigitalOcean (Droplet)
    
-   **Operating System:** Ubuntu Server
    
-   **Web Server:** Nginx (to be configured as a reverse proxy)
    
-   **Service Management:** Systemd (for managing the Kestrel process)    

_This section will be updated with full deployment instructions once the environment setup is complete._

## 🧪 Testing (In Progress) 🚧

A comprehensive testing strategy is currently being implemented to ensure system reliability and maintainability.

### Technologies

-   **Framework:** NUnit
    
-   **Mocking:** Moq
    
-   **Assertions:** NUnit Assertions
    

### Current Status

-   **Unit Tests:** Active development for the **Application Layer**.
    
    -   Testing Command & Query Handlers (e.g., Auth, Products).
        
    -   Validating business logic and validation rules.
        
-   **Integration Tests:** Planned for future updates.
    

To run the available tests:

```
dotnet test
```
_This section will be updated with full testing tools once the testing is complete._
