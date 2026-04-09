StaffTrackerManager
Overview

StaffTrackerManager is a backend solution for employee management, designed using Clean Architecture / Onion Architecture principles.
The project is structured to separate concerns, improve testability, and ensure scalability for future features.

Architecture

The solution follows Clean Architecture:

StaffTrackerManager.sln
├── Domain                 ← Core business entities and logic
├── Application            ← Use cases, services, and DTOs
├── Infrastructure         ← Concrete implementations (EF Core, Repositories, Database)
└── API                    ← Web API endpoints, authentication, and middleware

Dependecies
- .NET 8.0
- EF Core 8.0.8
- SQL Server
- JWT Authentication packages
- Swashbuckle (Swagger) for API documentation