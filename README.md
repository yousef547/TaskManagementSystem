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
- Unit Testing

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

# 📬 Postman Collection

## Download Collection

[⬇ Download Postman Collection](./postman/TaskManagement_collection.json)

---

## Import Into Postman

1. Open Postman
2. Click Import
3. Select:
   `TaskManagement.postman_collection.json`
4. Start testing APIs

---

# 🌐 Base URL

```bash
http://localhost:8000/api/v1
```

---

# 🐳 Docker URLs

## Swagger

```bash
http://localhost:8080/swagger
```

## API Base URL

```bash
http://localhost:8080/api/v1
```

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
 ├── tests
 │    └── TaskManagement.Application.UnitTests
 │
 ├── postman
 │    └── TaskManagement_collection.json
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

---

# Task Module

## Features

- Create Task
- Update Task Status
- Get Tasks By Project
- Delete Task

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

---

# Unified API Response

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

---

# API Versioning

The API supports URL versioning.

Example:

```text
/api/v1/project
/api/v1/task
```

---

# Redis Cache

Redis is used as a distributed caching layer.

## Cached Queries

- Get All Projects

---

# Auditing

The system automatically tracks:

- CreatedBy
- CreatedDate
- LastModifiedBy
- LastModifiedDate

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

# Database Migration

## Create Migration

```bash
dotnet ef migrations add InitialCreate \
--project src/TaskManagement.Infrastructure \
--startup-project src/TaskManagement.API
```

---

# Swagger

Swagger is enabled for API testing.

## Swagger URL

```text
http://localhost:8000/swagger
```

---

# 🧪 Unit Testing

The project includes unit tests for:

## Project Handlers

- CreateProjectCommandHandler
- UpdateProjectCommandHandler
- DeleteProjectCommandHandler
- GetAllProjectsQueryHandler
- GetProjectByIdQueryHandler

## Task Handlers

- CreateTaskCommandHandler
- UpdateTaskStatusCommandHandler
- DeleteTaskCommandHandler
- GetTasksByProjectQueryHandler

## Validator Tests

- CreateProjectDtoValidator
- UpdateProjectDtoValidator
- CreateTaskDtoValidator
- UpdateTaskStatusDtoValidator
- LoginRequestDtoValidator
- RegisterRequestDtoValidator

## Testing Technologies

- xUnit
- Moq
- FluentAssertions

## Run Tests

```bash
dotnet test
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
| xUnit | Unit Testing |
| Moq | Mocking |
| FluentAssertions | Assertions |

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
| POST | /api/v1/project |
| GET | /api/v1/project |
| GET | /api/v1/project/{id} |
| PUT | /api/v1/project |
| DELETE | /api/v1/project/{id} |

---

## Tasks

| Method | Endpoint |
|---|---|
| POST | /api/v1/task |
| PUT | /api/v1/task/status |
| GET | /api/v1/task/project/{projectId} |
| DELETE | /api/v1/task/{id} |

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
- Integration Testing
- Serilog Logging
- RabbitMQ
- Background Jobs
- SignalR Notifications
- Kubernetes Deployment
- CI/CD Pipeline

---

# Author

Developed using:

- Clean Architecture
- Modern .NET Practices
- Scalable Design Principles




# ⚠️ Redis Local Setup

If you are running the project locally without Docker Compose, you must run a Redis container manually.

## Run Redis Container

```bash
docker run -d --name redis-test -p 6379:6379 redis
```

## Verify Redis Is Running

```bash
docker ps
```

You should see:

```text
redis-test
```

---

# 📝 Notes

- Redis is required for distributed caching.
- The API uses Redis for caching project and task queries.
- Default Redis port:
  
```text
6379
```

# ⚙️ appsettings.json Configuration (Local)

If you are running locally, make sure Redis connection is configured correctly inside:

```text
src/TaskManagement.API/appsettings.json
```

## Example

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=TaskManagementDb;Trusted_Connection=True;TrustServerCertificate=True"
  },

  "Redis": {
    "ConnectionString": "localhost:6379"
  }
}
```


