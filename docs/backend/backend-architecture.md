# Backend Architecture

## Tech Stack

- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- JWT authentication

## Architectural Style

Use a simple vertical-slice approach inside the API project.

```text
MyFi.Api/
  Common/
  Features/
    Users/
    Categories/
    Expenses/
    Subscriptions/
    Dashboard/
```

## Slice Responsibilities

Each feature slice should keep related files close together:

- controller
- request and response DTOs
- handler or service classes
- entity and EF Core configuration when the data is feature-specific
- feature-local query or command logic

Keep only a few shared cross-cutting pieces outside the slice:

- `DbContext`
- generic repository
- JWT and password helpers
- MediatR pipeline behaviors
- middleware and shared ProblemDetails helpers

## Design Guidance

- keep controllers thin
- use MediatR to dispatch feature commands and queries from controllers
- keep feature logic inside the same slice instead of spreading it across projects
- avoid over-abstracting repositories if EF Core already fits the use case
- keep user scoping explicit in queries and command handling
- use FluentValidation near each command or query and run it through the MediatR pipeline

## Suggested Solution Layout

```text
backend/
  src/
    MyFi.Api/
      Common/
        Api/
        Behaviors/
        Persistence/
        Results/
        Security/
      Features/
        Users/
          Endpoints/
          Signup/
          Login/
          GetCurrentUser/
          Domain/
          Persistence/
          Shared/
  tests/
    MyFi.Api.UnitTests/
    MyFi.Api.IntegrationTests/
```

## Testing Split

- `MyFi.Api.UnitTests`: fast coverage for domain behavior, validation, handlers, security helpers, and result mapping
- `MyFi.Api.IntegrationTests`: endpoint wiring, auth flows, middleware behavior, and real PostgreSQL-backed runtime paths

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
