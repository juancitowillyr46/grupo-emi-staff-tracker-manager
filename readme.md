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

Domain Entities

The Domain layer includes the core employee management model:

- Employee
  - Represents an employee in the system.
  - Stores the current position, salary, and department reference.
  - Keeps a history of positions and project assignments.

- Department
  - Represents the department where employees belong.

- Project
  - Represents a project that employees can be assigned to.

- PositionHistory
  - Tracks the different positions an employee has held over time.

- EmployeeProject
  - Acts as the link between employees and projects.
  - Supports the many-to-many relationship between both entities.

Relationships

- Department 1 --- N Employee
- Employee 1 --- N PositionHistory
- Employee N --- N Project through EmployeeProject

Notes

- `CurrentPosition` is stored as an integer in `Employee`, as required by the test statement.
- `PositionHistory.Position` is stored as a string to keep the historical record descriptive.
- The code is organized using a Clean Architecture style with Domain, Application, Infrastructure, and API layers.
