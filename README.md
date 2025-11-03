# 📱 MyChatApp

A modern real-time chat application built with **.NET**, following **Clean Architecture** principles.  
It supports **real-time messaging** with SignalR, authentication with **JWT & Identity**, **MongoDB persistence**, and integrations like **Redis caching** and **Hangfire jobs**.

## 📂 Project Structure

```
MyChatApp.sln
├── src
│   ├── Core
│   │   ├── MyChatApp.Domain
│   │   └── MyChatApp.Application
│   ├── Infrastructure
│   │   ├── MyChatApp.Infrastructure.Persistence
│   │   ├── MyChatApp.Infrastructure.Identity
│   │   └── MyChatApp.Infrastructure.Shared
│   └── Presentation
│       └── MyChatApp.WebAPI
└── tests
    └── MyChatApp.Application.UnitTests
```

## 🏗️ Architecture Overview

The solution follows **Clean Architecture** and **CQRS** with **MediatR**.

### 1. **Domain (Core)**

-   💡 Business entities & rules (`User`, `Chat`, `ChatMember`, `Message`, `MessageAttachment`,`MessageDlivery`,`MessageReation`)
    
-   🎭 Enums (`ChatType` , `MemberRole` , `MessageType`,`MessageStatus`,`NotificationType`,`PrivacyLevel`,`UserStatus`)
    
-   🔔 Shared (`Result`, `Error`)
    
-   🗂️ Repository Interfaces (`IRepository`,`IUserRepository`, `IChatRepository` , `IMessageRepository`)
    

### 2. **Application (Use Cases)**

-   📝 CQRS: Commands & Queries (``)
    
-   ⚡ MediatR Handlers
    
-   📤 DTOs (``)
    
-   ✅ FluentValidation for request validation
    
-   🔄 AutoMapper for mapping between domain & DTOs
    
-   🌐 External Service Interfaces (``)
    

### 3. **Infrastructure (Details)**

-   **Persistence**
    
    -   MongoDB repositories (`MongoDbChatRepository`)
        
    -   EF Core + Identity (`ApplicationDbContext`)
        
    -   SQL migrations
        
-   **Identity**
    
    -   JWT & refresh token service
        
    -   ASP.NET Core Identity customization
        
-   **Shared**
    
    -   Hangfire jobs (cleanup, notifications)
        
    -   Redis caching (`ICacheService`)
        
    -   Email & file storage (future extensions)
        

### 4. **Presentation (Web API)**

-   🌍 ASP.NET Core Web API + SignalR
    
-   🎮 Controllers: Thin endpoints calling MediatR
    
-   🔔 SignalR Hubs (``)
    
-   ⚙️ Program.cs: DI, Swagger, Serilog, CORS, JWT, Hangfire setup
    
-   🛡️ Middleware: Error handling, rate limiter, logging

## ✅ Features

-   🔐 **Authentication & Authorization** (JWT + Identity + refresh tokens)
    
-   💬 **Real-time chat** (SignalR for groups & 1-to-1 messaging)
    
-   📦 **MongoDB persistence** for chat data
    
-   🗄️ **SQL Server with EF Core** for user & role management
    
-   🚀 **CQRS with MediatR** for clean use case separation
    
-   📜 **FluentValidation** for request validation
    
-   ⚡ **Redis caching** for performance
    
-   ⏱️ **Background jobs** with Hangfire
    
-   📊 **Logging** with Serilog + structured logs
    
-   📖 **Swagger/OpenAPI** documentation

## 🧪 Tests

-   **Unit Tests** → `MyChatApp.Application.UnitTests`

## 📌 Roadmap

-   🚧 Basic chat functionality
    
-   🚧 Authentication & authorization
    
-   🚧 File sharing (images, docs)
    
-   🚧 Message search & filters
    
-   🚧 Push notifications (mobile/web)

## 🤝 Contributing

Contributions are welcome! Please fork the repo and create a pull request.
