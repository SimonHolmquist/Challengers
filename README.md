# Challengers

A backend system for simulating tennis tournaments with knockout rules, supporting gender-specific logic, score explanation, and secure API access. Built with .NET 8 and Clean Architecture.

## Table of Contents

- [Project Overview](#project-overview)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Technologies Used](#technologies-used)
- [Testing](#testing)
- [Running Locally with Docker](#running-locally-with-docker)
- [Branches](#branches)
- [License](#license)

---

## Project Overview

This API simulates tennis tournaments using direct elimination. Matches are calculated based on player attributes and random luck. The system includes:

- Male and female tournament simulation logic
- Domain-driven modeling and layered architecture
- REST API with filtering and pagination
- JWT-based authentication for admin-only operations
- Multilanguage support via `.resx` and headers
- Integration with Docker and SQL Server
- Unit and integration tests

---

## Architecture

The project follows Clean Architecture and separation of concerns across multiple layers:

- Domain: core entities, value objects, business logic
- Application: CQRS handlers, validation, DTOs
- Infrastructure: database access (EF Core), JWT, DI
- API: ASP.NET Core REST endpoints, authentication, Swagger

---

## Project Structure

```
src/
  Challengers.Api             // ASP.NET Core Web API
  Challengers.Application     // CQRS handlers and services
  Challengers.Domain          // Entities and core logic
  Challengers.Infrastructure  // Persistence and authentication
  Challengers.Shared          // Enums, constants, helpers

tests/
  Challengers.UnitTests       // Domain and application tests
  Challengers.AuthTests       // JWT authentication tests
```

---

## Technologies Used

- [.NET 8](https://dotnet.microsoft.com/en-us/download)
- ASP.NET Core
- Entity Framework Core
- MediatR (CQRS)
- FluentValidation
- SQL Server (Docker)
- Swagger / OpenAPI
- JWT Authentication
- Localization (`.resx`)
- Docker & Docker Compose

---

## Testing

- Unit tests for:
  - `Player`, `Match`, `Tournament`
  - CQRS handlers: `CreateTournament`, `SimulateTournament`, `GetTournamentById`, etc.
- Integration tests for:
  - Auth endpoints (`/login`)
  - Authorization restrictions on protected endpoints
- Test configuration via `ENABLE_AUTH` environment variable

---

## Running Locally with Docker

1. Clone the repository
2. Run:
   ```bash
   docker-compose up --build
   ```
3. Swagger available at:
   [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)

---

## Branches

Development followed Git Flow:
- `main`: production-ready
- `develop`: integrated all features
- Feature branches:
  - `feature/domain-model`: domain layer
  - `feature/persistence`: EF Core + SQL Server
  - `feature/application-services`: CQRS + use cases
  - `feature/tournament-api`: REST API + filters
  - `feature/authentication`: JWT login and protection
  - `test/domain-and-handlers`: domain + application tests
  - `test/api-tests`: integration tests, authorization

---

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

## Author

Developed and maintained by Simón Holmquist.
