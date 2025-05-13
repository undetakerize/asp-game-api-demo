<p align= "center">
  <img src="logo-2.png" alt="ASP.NET CORE"/>
</p>

##

<p align= "center">
  ASP.NET Core codebase containing example (CRUD, auth, CQRS patterns architecture, etc).
</p>

##


#### This is a sample project for a maintainable store game database with user reviews built with ASP.NET Core. It includes CRUD operations, authentication, pagination, and more.
---

## Prepared Requirements
This is using ASP .NET Core With:
 - CQRS & [MediaTr](https://github.com/jbogard/MediatR)
   - Clean Architecture / Onion Architecture with elements of DDD.
   - Principle of CQRS (Command Query Responsibility Segregation) structure & clean architecture with support MediaTr as mediator call pattern.
 - [Entity Framework Core](https://docs.microsoft.com/en-us/ef/) on SQLite for demo purposes. Can easily be anything else EF Core supports. Open to porting to other ORMs/DBs.
 - JWT Authentication with [Identity Core](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-9.0&tabs=visual-studio).
 - [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) Built in swagger.
 - Integrated with database using [Npgsql](https://www.npgsql.org/)
 - Json Serializer/Deserialize using [Newtonsoft.JSON](https://www.newtonsoft.com/json) and it might be support [EFCore Naming Conventions](https://www.nuget.org/packages/EFCore.NamingConventions) for bind snake, camel , others.
 - Implemented Unit test with [Moq](https://www.nuget.org/packages/moq/), [NUnit](https://www.nuget.org/packages/nunit), [Ef Core InMemory](https://www.nuget.org/packages/microsoft.entityframeworkcore.inmemory).
 - Implemented [Confluent Kafka](https://www.nuget.org/packages/confluent.kafka/) with Pre-Configured Base for produce other service or even consume by this apps.
 - Using validation with [FluentValidation](https://www.nuget.org/packages/FluentValidation) , and behaviors [FluentValidationExtension](https://www.nuget.org/packages/fluentvalidation.dependencyinjectionextensions/).

**Notes** : Also this project runs on SDK 8. Make sure you have it!.

## The Structure 
The structure of this using clean architecture with principal CQRS.
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

## Installation/ Running Locals
Clone the project.
<pre>
git clone https://github.com/undetakerize/asp-game-api-demo.git
cd API
dotnet restore
dotnet build
dotnet run --project src/API
</pre>

## Config
Before running you should running set Environment Variables with CLI or your favorite IDE and make your own appsettings.Developement.Json or using appsettings.Json globaly.
Or config its launchSetting.json (do it with experimental too).
<pre>
launchSetting.json
</pre>
<pre>
..
"environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Production"
      }
..
</pre>

## Example appSettings.Json
Make sure you config the variable of connection db in appSettings.json / appSettings.Developement.json
<pre>
  {
  "ConnectionStrings": {
    "DefaultConnection": "Host={localhost:port};Database={database};Username={datbase.username};Password={database.password}"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "JWT": {
    "Issuer": "http://localhost:5246",
    "Audience": "http://localhost:5246",
    "SigningKey": "T6JzqP7eX9s8dGm2yL1q9Wv5Zr4yX7rL0e8cT1qK3bV9xF7pWq4gY2dN8rK0vH6a"
  }
}

</pre>
