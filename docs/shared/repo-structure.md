# Concrete Folder Structure

This is the recommended repository shape before implementation begins.

## Top-Level Layout

```text
myfi/
  docs/
    shared/
    frontend/
    backend/
  frontend/
  backend/
  .env.example
  docker-compose.yml
  README.md
```

## Frontend Structure

```text
frontend/
  src/
    app/
      providers/
      router/
    api/
    components/
      charts/
      forms/
      layout/
      feedback/
    features/
      auth/
      categories/
      dashboard/
      expenses/
      subscriptions/
    hooks/
    lib/
    pages/
    styles/
    types/
  public/
  index.html
  package.json
  tsconfig.json
  vite.config.ts
```

## Backend Structure

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
  MyFi.sln
```

## Backend Responsibility Split

- `MyFi.Api`: endpoints, middleware, auth plumbing, DI registration
- `MyFi.Application`: use cases, DTOs, validators, interfaces, query logic
- `MyFi.Domain`: entities, enums, value rules, core business invariants
- `MyFi.Infrastructure`: EF Core, database access, external provider integrations, token persistence

## Frontend Responsibility Split

- `app/`: providers, router, app bootstrap
- `api/`: HTTP client and API modules
- `features/`: resource-based screens, hooks, local view components, forms
- `components/`: shared UI elements reused across features
- `pages/`: route-level page composition
- `lib/`: utilities, formatters, auth storage helpers
- `types/`: shared frontend-only TypeScript models where needed

## Ownership Guidance

- Frontend dev owns `frontend/` and uses `docs/shared/` plus `docs/frontend/`
- Backend dev owns `backend/` and uses `docs/shared/` plus `docs/backend/`
- Contract changes must be updated first in `docs/shared/api-contract.md`
