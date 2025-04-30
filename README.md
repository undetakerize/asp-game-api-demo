<p style="text-align: center">
  <img src="logo-2.png" alt="ASP.NET CORE"/>
</p>

---

<p style="text-align: center">
  ASP.NET Core codebase containing example (CRUD, auth, CQRS patterns architecture, etc).
</p>

---


This is a sample project for a maintainable game database built with ASP.NET Core. It includes CRUD operations, authentication, pagination, and more.

---

## Prepared Requirements

---
This is using ASP .NET Core With:
 - CQRS & [MediaTr](https://github.com/jbogard/MediatR)
   - Clean Architecture / Onion Architecture with elements of DDD.
   - Principle of CQRS (Command Query Responsibility Segregation) structure & clean architecture with support MediaTr as mediator call pattern.
 - [Entity Framework Core](https://docs.microsoft.com/en-us/ef/) on SQLite for demo purposes. Can easily be anything else EF Core supports. Open to porting to other ORMs/DBs.
 - [MediaTr](https://github.com/jbogard/MediatR) 
 - JWT Authentication with [Identity Core](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-9.0&tabs=visual-studio).
 - [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) Built in swagger.
 - Integrated with database using [Npgsql](https://www.npgsql.org/)
 - Json Serializer/Deserialize using [Newtonsoft.JSON](https://www.newtonsoft.com/json) and it might be support [EFCore Naming Conventions](https://www.nuget.org/packages/EFCore.NamingConventions) for bind snake, camel , others.

## The Structure 

---
<pre> 
/src
│
├── API                        → Presentation layer (Controllers, DI, middleware)
│   └── Controllers
│       ├── UsersController.cs
│       └── UserDetailsController.cs
│
├── Application                → Application layer (Use cases, DTOs)
│   ├── Users
│   │   ├── Commands
│   │   │   └── CreateUserCommand.cs
│   │   ├── Queries
│   │   │   └── GetUserByIdQuery.cs
│   │   ├── DTOs
│   │   └── Interfaces
│   │       └── IUserService.cs
│   └── UserDetails
│       ├── Commands
│       │   └── CreateUserDetailCommand.cs
│       ├── DTOs
│       └── Interfaces
│           └── IUserDetailService.cs
│
├── Domain                    → Domain layer (Entities, Interfaces)
│   ├── Users
│   │   ├── User.cs
│   │   └── IUserRepository.cs
│   └── UserDetails
│       ├── UserDetail.cs
│       └── IUserDetailRepository.cs
│
├── Infrastructure           → Infrastructure layer (EF Core, Repos)
│   ├── Persistence
│   │   ├── AppDbContext.cs
│   │   ├── UserRepository.cs
│   │   └── UserDetailRepository.cs
│   └── Services (e.g., Email, Logging)
├── Middleware
└── SharedKernel (optional)  → Shared base classes, utilities, interfaces
</pre>
