# Task Management System API

## Overview

Task Management System is a production-ready backend API built with:

- .NET 9
- ASP.NET Core Web API
- Clean Architecture
- CQRS
- MediatR
- Entity Framework Core
- SQL Server
- Redis Cache
- JWT Authentication
- Docker
- Swagger
- FluentValidation
- Global Exception Handling
- API Versioning

The system provides:

- Authentication & Authorization
- Project Management
- Task Management
- Role-Based Access Control
- Redis Distributed Caching
- Auditing
- Unified API Responses
- Dockerized Environment

---

# Architecture

The project follows Clean Architecture principles.

## Project Structure

```text
TaskManagementSystem
 ├── src
 │    ├── TaskManagement.API
 │    ├── TaskManagement.Application
 │    ├── TaskManagement.Domain
 │    └── TaskManagement.Infrastructure
 │
 ├── docker-compose.yml
 └── TaskManagementSystem.sln
```

---

# Layers

## Domain Layer

Contains:

- Entities
- Enums
- Base Classes
- Business Rules

Example:

- Project
- ProjectTask
- EntityBase
- ProjectTaskPriority
- ProjectTaskStatus

---

## Application Layer

Contains:

- CQRS
- Commands
- Queries
- DTOs
- Interfaces
- Validators
- MediatR Handlers
- Business Logic

Technologies:

- MediatR
- FluentValidation

---

## Infrastructure Layer

Contains:

- Entity Framework Core
- JWT Authentication
- Redis Cache
- DbContext
- Persistence
- Seeders
- External Services

---

## API Layer

Contains:

- Controllers
- Middleware
- Swagger
- Dependency Injection
- API Configuration

---

# Features

## Authentication & Authorization

### JWT Authentication

The API uses JWT Bearer Authentication.

### Roles

- Admin
- Manager
- User

### Seeded Users

| Role | Email | Password |
|---|---|---|
| Admin | admin@taskmanagement.com | Admin123! |
| Manager | manager@taskmanagement.com | Manager123! |
| User | user@taskmanagement.com | User123! |

---

# Project Module

## Features

- Create Project
- Get All Projects
- Get Project By Id
- Update Project
- Delete Project

## Project Entity

```csharp
public class Project
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }
}
```

---

# Task Module

## Features

- Create Task
- Update Task Status
- Get Tasks By Project
- Delete Task

## Task Entity

```csharp
public class ProjectTask
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public ProjectTaskStatus Status { get; set; }

    public DateTime DueDate { get; set; }

    public ProjectTaskPriority Priority { get; set; }

    public Guid ProjectId { get; set; }
}
```

---

# CQRS & MediatR

The application follows the CQRS pattern.

## Commands

Used for:

- Create
- Update
- Delete

Examples:

- CreateProjectCommand
- UpdateProjectCommand
- DeleteTaskCommand

## Queries

Used for:

- Read operations

Examples:

- GetAllProjectsQuery
- GetTasksByProjectQuery

---

# Validation

FluentValidation is used for validating:

- DTOs
- Commands
- Requests

Example:

```csharp
public class LoginRequestDtoValidator
    : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}
```

---

# Unified API Response

All APIs return unified responses.

## Success Response

```json
{
  "success": true,
  "message": "Success",
  "cacheKey": null,
  "data": {},
  "errors": null
}
```

## Error Response

```json
{
  "success": false,
  "message": "Validation Failed",
  "cacheKey": null,
  "data": null,
  "errors": [
    "Email is required"
  ]
}
```

---

# Global Exception Handling

The API uses custom middleware for:

- Exception Handling
- Logging
- Error Response Formatting

Features:

- Centralized exception handling
- Standardized error responses
- Cleaner controllers

---

# API Versioning

The API supports URL versioning.

Example:

```text
/api/v1/projects
/api/v1/tasks
```

---

# Redis Cache

Redis is used as a distributed caching layer.

## Cached Queries

- Get All Projects
- Get Tasks By Project

## Benefits

- Faster responses
- Reduced SQL Server load
- Improved scalability

## Caching Flow

```text
Request
   ↓
Check Redis
   ↓
Exists? → Return Cache
   ↓
Else → SQL Server
   ↓
Save To Redis
   ↓
Return Response
```

---

# Auditing

The system automatically tracks:

- CreatedBy
- CreatedDate
- LastModifiedBy
- LastModifiedDate

The current authenticated user is automatically resolved using:

- IHttpContextAccessor
- ClaimsPrincipal

---

# Docker Support

The application is fully dockerized.

## Containers

- API Container
- SQL Server Container
- Redis Container

## Run Application

```bash
docker compose up --build
```

---

# Docker Compose

```yaml
services:

  taskmanagement-api:
    build:
      context: .
      dockerfile: src/TaskManagement.API/Dockerfile

  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest

  redis:
    image: redis:latest
```

---

# Database Migration

## Create Migration

```bash
dotnet ef migrations add InitialCreate \
--project src/TaskManagement.Infrastructure \
--startup-project src/TaskManagement.API
```

## Update Database

```bash
dotnet ef database update \
--project src/TaskManagement.Infrastructure \
--startup-project src/TaskManagement.API
```

---

# Swagger

Swagger is enabled for API testing.

## Swagger URL

```text
http://localhost:5093/swagger
```

---

# Technologies Used

| Technology | Purpose |
|---|---|
| .NET 9 | Backend Framework |
| ASP.NET Core | Web API |
| Entity Framework Core | ORM |
| SQL Server | Database |
| Redis | Distributed Cache |
| MediatR | CQRS |
| FluentValidation | Validation |
| JWT | Authentication |
| Docker | Containerization |
| Swagger | API Documentation |

---

# Security

Implemented security features:

- JWT Authentication
- Role-Based Authorization
- Validation
- Secure Password Hashing
- Protected Endpoints

---

# Future Improvements

Possible future enhancements:

- Refresh Tokens
- Unit Testing
- Integration Testing
- Serilog Logging
- RabbitMQ
- Background Jobs
- SignalR Notifications
- Kubernetes Deployment
- CI/CD Pipeline

---

# Running Locally

## Prerequisites

- .NET 9 SDK
- Docker Desktop
- SQL Server

## Run Application

```bash
dotnet restore
```

```bash
dotnet build
```

```bash
dotnet run --project src/TaskManagement.API
```

---

# Running With Docker

```bash
docker compose up --build
```

---

# API Endpoints

## Authentication

| Method | Endpoint |
|---|---|
| POST | /api/v1/auth/register |
| POST | /api/v1/auth/login |

---

## Projects

| Method | Endpoint |
|---|---|
| POST | /api/v1/projects |
| GET | /api/v1/projects |
| GET | /api/v1/projects/{id} |
| PUT | /api/v1/projects |
| DELETE | /api/v1/projects/{id} |

---

## Tasks

| Method | Endpoint |
|---|---|
| POST | /api/v1/tasks |
| PUT | /api/v1/tasks/status |
| GET | /api/v1/tasks/project/{projectId} |
| DELETE | /api/v1/tasks/{id} |

---

# Author

Developed using:

- Clean Architecture
- Enterprise Patterns
- Modern .NET Practices
- Scalable Design Principles

