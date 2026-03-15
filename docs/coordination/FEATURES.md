# Feature Definitions

This file holds the current implementation-ready feature and story entries.

## SETUP-003 - Initialize frontend and backend project scaffolds

- Status: `In Progress`
- Suggested branch: `chore/project-scaffolds`
- Goal: turn the placeholder workspaces into real app foundations without moving
  away from the documented stack and folder boundaries.
- Scope:
  - initialize the React + TypeScript + Vite frontend inside `frontend/`
  - keep the checked-in ASP.NET Core backend scaffold aligned with the docs while frontend setup catches up
  - add the first `.env.example` and any minimal root setup files needed to run locally
  - keep code and config aligned with `docs/shared/repo-structure.md`
- Acceptance criteria:
  - `frontend/` contains the planned app scaffold structure
  - `backend/` contains the current documented solution/project structure
  - the repository still reflects the documented monorepo shape
  - docs are updated if the real scaffold introduces small naming adjustments
- Docs to update:
  - `README.md`
  - `docs/shared/repo-structure.md`
  - any workspace README that changes because of real scaffold details

## AUTH-001 - Build backend auth foundation

- Status: `In Progress`
- Suggested branch: `feat/backend-auth-foundation`
- Depends on: `SETUP-003`
- Goal: establish the backend auth foundation with local email/password, backend-issued JWTs, and a path to add external providers later.
- Scope:
  - add the user, password, token, and persistence foundation for local auth
  - expose signup, login, and `users/me` as the first backend auth slice
  - wire validation, ProblemDetails responses, API versioning, and dev API docs
  - keep future provider-specific logic behind backend boundaries
- Acceptance criteria:
  - the backend contains the first auth flow scaffolding matching `docs/shared/auth-flow.md`
  - auth models align with `docs/shared/domain-model.md`
  - endpoint names and payloads align with `docs/shared/api-contract.md`
  - unit tests cover auth domain, validation, handler, and security logic
  - integration tests prove the main auth flow against PostgreSQL-backed runtime behavior
- Docs to update:
  - `docs/shared/api-contract.md` if DTOs move
  - `docs/shared/auth-flow.md` if flow details move
  - backend docs if implementation constraints are discovered

## AUTH-002 - Build frontend auth shell

- Status: `Backlog`
- Suggested branch: `feat/frontend-auth-shell`
- Depends on: `SETUP-003`, `AUTH-001`
- Goal: create the frontend auth shell that can handle login, store session state, and protect authenticated routes.
- Scope:
  - add app bootstrap and auth provider wiring
  - add login entry point and initial authenticated route flow
  - add protected route behavior for authenticated pages
  - align token handling with backend-issued app auth
- Acceptance criteria:
  - a user can reach a login screen and establish an authenticated session
  - protected routes can distinguish authenticated vs unauthenticated states
  - frontend auth code uses the backend auth contract rather than Google client SDK assumptions
  - page and component organization follows the frontend architecture docs
- Docs to update:
  - `docs/shared/api-contract.md` only if the frontend discovers a contract gap
  - frontend docs if route or provider structure changes materially

## CAT-001 - Category CRUD vertical slice

- Status: `Backlog`
- Suggested branch: `feat/category-crud`
- Depends on: `SETUP-003`, `AUTH-001`, `AUTH-002`
- Goal: let an authenticated user create, edit, list, and delete categories.
- Acceptance criteria:
  - authenticated category CRUD works end to end
  - category names are unique per user
  - validation and error states match the shared contract and frontend patterns

## EXP-001 - Expense CRUD vertical slice

- Status: `Backlog`
- Suggested branch: `feat/expense-crud`
- Depends on: `CAT-001`
- Goal: let an authenticated user manage one-time expenses.
- Acceptance criteria:
  - expense CRUD works end to end
  - expense money values use decimal-safe backend types
  - filtering and sorting hooks leave room for later expansion

## SUB-001 - Subscription CRUD vertical slice

- Status: `Backlog`
- Suggested branch: `feat/subscription-crud`
- Depends on: `CAT-001`
- Goal: let an authenticated user manage recurring subscriptions.
- Acceptance criteria:
  - subscription CRUD works end to end
  - billing cycle and renewal date behavior match the domain model
  - inactive subscriptions remain stored for history

## DASH-001 - Dashboard summary vertical slice

- Status: `Backlog`
- Suggested branch: `feat/dashboard-summary`
- Depends on: `EXP-001`, `SUB-001`
- Goal: provide a useful monthly dashboard summary across expenses and subscriptions.
- Acceptance criteria:
  - backend summary endpoint returns user-scoped aggregates only
  - frontend dashboard communicates current month activity clearly
  - empty and loading states are handled intentionally

## QUAL-001 - MVP hardening pass

- Status: `Backlog`
- Suggested branch: `chore/mvp-hardening`
- Depends on: `AUTH-001`, `AUTH-002`, `CAT-001`, `EXP-001`, `SUB-001`, `DASH-001`
- Goal: tighten the MVP after the main flows exist.
- Scope:
  - validation gaps
  - error responses and error UI
  - loading and empty states
  - test coverage for core business rules and integration paths
- Acceptance criteria:
  - the main MVP flows feel stable and coherent
  - obvious contract mismatches are resolved
  - test coverage improves where it gives the most confidence

## Phase 2 Features

These items stay out of active MVP implementation until the core flows are stable.

- `BUD-001` - budgets foundation
- `REP-001` - richer charts and reports
- `REM-001` - in-app reminders
- `CSV-001` - CSV import/export
