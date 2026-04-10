# StaffTrackerManager

## Overview

StaffTrackerManager is a backend solution for employee management, designed with Clean Architecture / Onion Architecture principles.
The project separates concerns, keeps the domain isolated, and makes the solution easier to evolve and test.

## Development Approach

I started by reading the full test carefully to understand the scope and the expected outcomes before writing any code.

From there, I focused on the data flow first so I could define a database model that would support the requirements with enough room to grow.

Next, I broke the work into small user stories to keep the implementation incremental and easier to validate step by step.

With that in place, I chose a hexagonal-style architecture and kept the structure simple so the solution could scale without becoming hard to follow.

I then developed the project from the inside out:

- Domain
- Application
- Infrastructure
- API

This approach helped keep the core logic isolated while still allowing the presentation and persistence layers to evolve around it.

Throughout the implementation, I applied SOLID principles and used design patterns to keep the application loosely coupled and easier to maintain.

## User Stories

The implementation was organized around small user stories based on the test requirements:

- As a user, I want to see a list of employees so I can review the current workforce.
- As a user, I want to get a single employee by id so I can inspect their details.
- As an admin, I want to create employees so I can manage the employee directory.
- As an admin, I want to update employees so I can keep their data current.
- As an admin, I want to delete employees so I can remove records when needed.
- As an admin, I want to assign multiple projects to an employee so the many-to-many relationship is easy to manage.
- As a user, I want to filter employees by department and project participation so I can verify the LINQ query from the test.
- As a user, I want to register and log in with JWT so I can access protected endpoints.
- As an admin, I want role-based access control so I can protect write operations.
- As a developer, I want request logging middleware so I can trace incoming HTTP calls.

## Architecture

The solution follows a layered architecture:

StaffTrackerManager.sln
├── Domain                 <- Core business entities and logic
├── Application            <- Use cases, contracts, and DTOs
├── Infrastructure         <- EF Core, repositories, persistence, and auth storage
└── API                    <- Web API endpoints, JWT auth, and middleware

## Dependencies

- .NET 8.0
- EF Core 8.0.8
- SQL Server Express
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.EntityFrameworkCore.Design
- Swashbuckle.AspNetCore
- System.Security.Cryptography (used by the password hasher)
- Swashbuckle (Swagger) for API documentation

### Security

The API uses JWT for authentication and role-based authorization for access control.

Passwords are not encrypted in the database. They are hashed with SHA-256 and stored as a hexadecimal string.
When a user logs in, the provided password is hashed again and compared with the stored hash.
That keeps the implementation simple for the technical test while avoiding plain-text password storage.

## Domain Entities

The Domain layer contains the core employee management model:

- Employee
  - Represents an employee in the system.
  - Stores the current position, salary, and department reference.
  - Keeps a history of positions and project assignments.
  - Exposes the yearly bonus calculation required by the test.

- Department
  - Represents the department where employees belong.

- Project
  - Represents a project that employees can be assigned to.

- PositionHistory
  - Tracks the different positions an employee has held over time.

- Position
  - Represents the current position catalog used by employees.
  - Stores the position name and whether it counts as a manager role.

- EmployeeProject
  - Acts as the link between employees and projects.
  - Supports the many-to-many relationship between both entities.

- User
  - Represents authenticated users for JWT login and registration.

## Relationships

- Department 1 --- N Employee
- Employee 1 --- N PositionHistory
- Employee N --- N Project through EmployeeProject

## Notes

- `CurrentPosition` is stored as an integer in `Employee`, as required by the test statement.
- `CurrentPosition` points to the `Positions` catalog, which makes the bonus calculation easier to maintain.
- Manager roles are grouped through `PositionType`, so several manager titles can share the same bonus rule.
- `PositionHistory.Position` is stored as a string to keep the historical record descriptive.
- The code follows Clean Architecture with Domain, Application, Infrastructure, and API layers.

## Section 1: C# Programming

The `Employee` entity includes the yearly bonus calculation required by the statement.
The method calculates:

- 10% of salary for regular employees
- 20% of salary for managers

The current position comes from the `Positions` catalog, which keeps the rule easy to extend if the company adds new manager titles later on.

## Section 2: ASP.NET Core

### 2.1 Web API endpoints

The API exposes the required employee endpoints:

- `GET /api/employees`
  - Returns all employees.

- `GET /api/employees/{id}`
  - Returns one employee by id.

- `POST /api/employees`
  - Creates a new employee.

- `PUT /api/employees/{id}`
  - Updates an existing employee.

- `DELETE /api/employees/{id}`
  - Deletes an employee.

Additional employee endpoints:

- `GET /api/employees/department/{departmentId}/with-projects`
  - Returns employees from a specific department that have at least one assigned project.

- `POST /api/employees/{employeeId}/projects`
  - Assigns one or more projects to an employee.

### 2.2 Authentication and authorization

Authentication is handled with JWT tokens.

- Users can register and log in through the auth controller.
- After a successful login, the API returns a JWT token.
- The token includes the username and role claims.
- Protected endpoints expect the token in the `Authorization` header.

Authorization is role-based:

- `Admin`
  - Can use all employee endpoints.

- `User`
  - Can only access the `GET` employee endpoints.

This keeps security concerns out of the business logic and fits well with the chosen architecture.

### 2.3 Middleware

Middleware in ASP.NET Core is code that runs in the HTTP request pipeline.
It can inspect, change, or stop a request before it reaches the controller.

In this solution, I added a simple request logging middleware that writes the method and path of each incoming request.
That keeps logging out of the controllers and makes the API easier to follow while debugging.

## Section 3: Authentication and Authorization

The authentication controller exposes:

- `POST /api/auth/register`
- `POST /api/auth/login`

The implementation uses:

- SQL-backed user storage
- password hashing with SHA-256
- JWT token generation
- role-based authorization

This keeps the auth flow simple while still being persistent and testable.

## Section 4: Database Design and EF Core

### Database schema

The SQL schema includes:

- `Employees`
- `Departments`
- `Projects`
- `PositionHistory`
- `EmployeeProjects`
- `Users`

The relationships are:

- `Departments 1 --- N Employees`
- `Employees 1 --- N PositionHistory`
- `Employees N --- N Projects` through `EmployeeProjects`

### EF Core implementation

The Infrastructure layer contains:

- `AppDbContext`
  - Exposes the database sets for all entities.
  - Applies all EF Core configurations from the assembly.

- Entity configurations
  - `EmployeeConfiguration`
  - `DepartmentConfiguration`
  - `ProjectConfiguration`
  - `PositionHistoryConfiguration`
  - `EmployeeProjectConfiguration`
  - `UserConfiguration`

- Repositories
  - `EmployeeRepository`
  - `DepartmentRepository`
  - `ProjectRepository`
  - `UserStore`

- `UnitOfWork`
  - Wraps `SaveChangesAsync()` for coordinated persistence.

### LINQ query required by the statement

The repository contains a LINQ query that fetches employees who:

- belong to a specific department
- and are assigned to at least one project

The query is exposed through:

- `GET /api/employees/department/{departmentId}/with-projects`

### Seed data

The database includes seed data for:

- departments
- projects
- users for authentication

## Section 5: Performance and Optimization

### Common performance issues in .NET applications

- Too many database round trips
- Pulling more data than needed
- Missing database indexes
- Blocking calls in hot paths
- Extra object allocations
- Using tracking queries when the data is read-only

### How to address them

- Use async I/O for database and network calls
- Select only the fields you really need
- Add indexes for the filters and joins you use often
- Use `AsNoTracking()` for read-only queries
- Avoid N+1 queries with proper `Include` usage or projection
- Cache data that is expensive to build but does not change often

### How to profile and optimize a slow-running query

- Start by finding the slow query in logs, APM tools, or SQL Server profiling.
- Review the SQL generated by EF Core.
- Check the execution plan in SQL Server.
- Look for missing indexes or full table scans.
- Reduce over-fetching by projecting only the fields you need.
- Measure again after each change so you know what actually helped.

## Additional Requirements

- SOLID principles
  - Responsibilities are separated across Domain, Application, Infrastructure, and API.
  - Dependencies point inward toward abstractions.

- Architectural pattern
  - Clean Architecture / Onion Architecture is used.

- Design patterns
  - Repository Pattern
  - Unit of Work Pattern
  - JWT-based authentication flow also follows a clear service abstraction approach

## How to Run

1. Update the connection string in `API/appsettings.json` if needed.
2. Run migrations:

```powershell
dotnet ef database update --project .\Infrastructure\Infrastructure.csproj --startup-project .\API\API.csproj
```

3. Start the API:

```powershell
dotnet run --project .\API\API.csproj
```

4. Use Swagger or `API.http` to test the endpoints.

## Final Notes

- `CurrentPosition` is kept as an integer because the exercise explicitly requires it.
- The bonus is derived from the employee state, so it is calculated at runtime instead of being stored in the database.
- The project keeps the implementation simple on purpose so it is easy to review and explain during the test.
