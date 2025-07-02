# 🗂️ Task Tracker Application

A full-stack task management system built with ASP.NET Core Web API and Angular.

---

## 🔧 Tech Stack

- **Backend**: ASP.NET Core (.NET 8)
- **Frontend**: Angular 18
- **ORM**: Entity Framework Core
- **Database**: SQL Server
- **Testing**: xUnit, Moq, FluentAssertions
- **Design Pattern**: Repository & Unit of Work

---

## 📁 Project Structure

| Project                  | Description                                 |
|--------------------------|---------------------------------------------|
| `TaskTrackerAPI`         | Main API Project                            |
| `TaskTracker.Domain`     | Core domain models                          |
| `TaskTracker.Application`| DTOs, services, and interfaces              |
| `TaskTracker.Infrastructure` | EF Core Repositories & UnitOfWork       |
| `TaskTracker.Tests`      | Unit tests using xUnit                      |
| `task-tracker-ui`        | Angular 18 frontend (Bootstrap UI)          |

---

## 🧩 Design Decisions

- **Clean Architecture** separation for maintainability.
- **AutoMapper** used for DTO ↔ Entity mapping.
- **Keyword-based Task Categorization**.
- **DTO Validation** using DataAnnotations.
- **In-Memory DB for Testing** to ensure isolation and fast execution.
- **Swagger/OpenAPI documentation**
- **Global exception middleware**

---

## 📦 API Endpoints

- `GET /api/tasks/getAllTasks` – Fetch all tasks
- `GET /api/tasks/getTasks?pageNumber=1&pageSize=10` – Paged task fetch
- `GET /api/tasks/getTask/{id}` – Fetch task by ID
- `POST /api/tasks/createTask` – Create new task
- `PUT /api/tasks/updateTask/{id}` – Update existing task
- `DELETE /api/tasks/deleteTask/{id}` – Delete task

Returns appropriate HTTP codes: `200`, `201`, `204`, `404`, `500`

---

## 📌 Assumptions

- `TaskPriority` is passed as an integer.
- `IsCompleted` is optional for updates.
- `Authentication` No authentication or authorization is required in current scope.
- `Filter` user-specific filtering was required for tasks.
---

## 🧪 Testing Strategy

- DTOs: Validated via `ValidationContext` and xUnit.
- Services: Mocked `UnitOfWork` and repositories using Moq.
- Repository: Tested with EF InMemory DB.

---

## 🚧 Roadmap

- [ ] Enable CI/CD (GitHub Actions / Azure DevOps)

---
