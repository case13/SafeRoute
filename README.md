# SafeRoute

SafeRoute is a BIM compliance validation system designed to automatically verify technical and safety rules in Revit-based projects.

The system analyzes BIM models and detects rule violations such as minimum door width, ramp inclination, and other regulatory requirements related to accessibility and fire safety.

---

## Purpose

To automate compliance validation in BIM projects, reducing human error and improving auditing efficiency.

---

## Key Features

- JWT Authentication with Refresh Token support
- Forced password change on first login
- Multi-tenant architecture (Company-based isolation)
- User management per company
- Administrative dashboard
- Clean layered architecture (Domain, Application, Infrastructure, API, Blazor)
- Extensible rule validation engine (doors and ramps implemented as initial scope)

---

## Architecture

The solution follows a layered architecture:

- Domain: Entities and business rules
- Application: Services and DTOs
- Infrastructure: EF Core, repositories, security implementations
- API: REST controllers
- Blazor Server: Frontend using MVVM pattern

Authentication is based on:

- JWT access tokens
- Persistent refresh tokens
- Custom claims
- Mandatory password update on first login

---

## Technology Stack

- .NET 8
- ASP.NET Core Web API
- Blazor Server
- Entity Framework Core
- SQL Server
- MudBlazor
- JWT Authentication
- PBKDF2 password hashing

---

## Security

- Password hashing with PBKDF2 + Salt
- Constant-time comparison using CryptographicOperations.FixedTimeEquals
- Refresh token expiration control
- Custom claim to enforce first-login password update

---

## How to Run

Backend:

cd SafeRoute.Api  
dotnet run

Frontend:

cd SafeRoute.BlazorServer  
dotnet run

Update the connection string in appsettings.json before running the application.

---

## Project Status

Actively under development.

Next milestone: automated BIM rule validation for doors and ramp compliance.

---

## Author

Anderson Gonçalves  
Fullstack .NET Developer