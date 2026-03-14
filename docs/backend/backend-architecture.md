# Backend Architecture

## Tech Stack

- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- FluentValidation
- JWT authentication

## Architectural Style

Use a pragmatic layered or simplified Clean Architecture approach.

```text
API -> Application -> Domain -> Infrastructure
```

## Layer Responsibilities

### API Layer

- HTTP endpoints
- authentication and authorization wiring
- request/response mapping
- global exception handling
- middleware registration

### Application Layer

- use cases and orchestration
- DTOs
- validation
- query services
- interfaces for infrastructure dependencies

### Domain Layer

- entities
- enums
- business invariants
- domain concepts that should not depend on frameworks

### Infrastructure Layer

- EF Core `DbContext`
- repositories or query implementations if used
- Google OAuth integration
- JWT generation
- refresh token persistence

## Design Guidance

- keep controllers thin
- keep business rules in application/domain, not in controllers
- avoid over-abstracting repositories if EF Core already fits the use case
- keep user scoping explicit in queries and command handling

## Suggested Solution Layout

```text
backend/
  src/
    MyFi.Api/
    MyFi.Application/
    MyFi.Domain/
    MyFi.Infrastructure/
  tests/
    MyFi.Api.IntegrationTests/
    MyFi.Application.Tests/
```

## Initial Application Modules

- Auth
- Users
- Categories
- Expenses
- Subscriptions
- Dashboard

## Cross-Cutting Concerns

- auth and current user access
- validation
- consistent error responses
- pagination model
- logging
