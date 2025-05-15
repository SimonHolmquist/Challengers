# Challengers 🎾

A tennis tournament simulator built with .NET 8, using Clean Architecture, SQL Server, and Docker.

## 🚀 Getting Started

1. Clone this repository
2. Run the following command:
   ```bash
   docker-compose up --build
   ```
3. Access the API at:
   [http://localhost:5000/swagger](http://localhost:5000/swagger)

## 📁 Project Structure

- `src/Challengers.Api`: ASP.NET Core Web API
- `src/Challengers.Domain`: Domain models and entities
- `src/Challengers.Application`: Use cases, services, and contracts
- `src/Challengers.Infrastructure`: EF Core and persistence logic

## 🧱 Architecture

This project follows Clean Architecture principles and uses Git Flow branching strategy:
- `main`: production-ready code
- `develop`: integration branch for features
- `feature/<name>`: individual feature branches

## ⚙️ Technologies

- .NET 8
- SQL Server (via Docker)
- Entity Framework Core
- Swagger / OpenAPI
- Docker & Docker Compose
