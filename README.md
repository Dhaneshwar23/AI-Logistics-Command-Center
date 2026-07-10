# AI Logistics Command Center

A production-ready ASP.NET Core 8 Web API that simulates a modern logistics management platform. The project is being built incrementally using enterprise development practices such as Clean Architecture, JWT authentication, Azure deployment, CI/CD, and automated testing.

---

## Technology Stack

### Backend

- ASP.NET Core 8
- C#
- Entity Framework Core
- SQL Server
- Azure SQL Database

### Architecture

- Clean Architecture
- Repository Pattern
- Service Layer
- Dependency Injection

### Security

- JWT Authentication
- Refresh Token Authentication
- Role-Based Authorization
- BCrypt Password Hashing

### Cloud & DevOps

- Azure App Service
- Azure SQL Database
- GitHub Actions CI/CD

### Testing

- xUnit
- Moq
- FluentAssertions

---

## Features Implemented

- Customer Management
- Shipment Management
- Tracking Events
- JWT Authentication
- Refresh Token Rotation
- Logout & Token Revocation
- Role-Based Authorization
- Admin User Seeding
- Global Exception Handling
- Request/Response Logging
- Correlation ID Middleware
- Serilog Logging
- Azure Deployment
- Azure SQL Database
- GitHub Actions CI/CD
- Unit Testing

---

## Project Structure

```
src/
    AILogistics.Api
    AILogistics.Application
    AILogistics.Domain
    AILogistics.Infrastructure

tests/
    AILogistics.Tests

docs/
    Architecture.md
```

---

## Current Version

**v1.3.0**

Completed:

- Backend Foundation
- Authentication
- Azure Deployment
- Authorization
- Refresh Token Authentication

---

## Upcoming Features

- API Versioning
- Rate Limiting
- Response Caching
- Health Checks
- Security Headers
- Pricing Engine
- Invoice Generation
- Azure Blob Storage
- Redis
- AI Shipment Assistant

---

## Documentation

Additional technical documentation is available in the **docs** folder.

```
docs/
    Architecture.md
```