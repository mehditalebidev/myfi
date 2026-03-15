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

- Status: `In Progress`
- Suggested branch: `feat/category-crud-backend`
- Depends on: `SETUP-003`, `AUTH-001`
- Goal: let an authenticated user create, edit, list, and delete categories.
- Backend-first note: implement the API slice and tests now while frontend work is deferred; complete the frontend hookup later under `AUTH-002` and the eventual authenticated app shell.
- Acceptance criteria:
  - authenticated backend category CRUD works against the documented contract
  - category names are unique per user
  - validation and error states match the shared contract and backend patterns now, with frontend hookup to follow later

## EXP-001 - Expense CRUD vertical slice

- Status: `In Progress`
- Suggested branch: `feat/expense-crud-backend`
- Depends on: `CAT-001`
- Goal: let an authenticated user manage one-time expenses.
- Backend-first note: implement the API slice and tests now while frontend work is deferred; complete the frontend hookup later under `AUTH-002` and the eventual authenticated app shell.
- Acceptance criteria:
  - authenticated backend expense CRUD works against the documented contract, including list/query support
  - expense money values use decimal-safe backend types
  - category references stay optional but must belong to the current user when supplied

## SUB-001 - Subscription CRUD vertical slice

- Status: `In Progress`
- Suggested branch: `feat/subscription-crud-backend`
- Depends on: `CAT-001`
- Backend-first note: the backend slice can land now on the documented contract while the frontend subscription screens remain deferred.
- Goal: let an authenticated user manage recurring subscriptions.
- Acceptance criteria:
  - authenticated backend subscription CRUD works against the documented contract
  - list endpoint supports pagination, search, active-status filtering, and renewal-date/amount sorting
  - category references stay optional but must belong to the current user when supplied
  - billing cycle and renewal date behavior match the domain model
  - inactive subscriptions remain stored for history

## DASH-001 - Dashboard summary vertical slice

- Status: `In Progress`
- Suggested branch: `feat/dashboard-summary-backend`
- Depends on: `EXP-001`, `SUB-001`
- Backend-first note: implement the dashboard summary backend projection now while frontend dashboard screens remain deferred.
- Goal: provide a useful monthly dashboard summary across expenses and subscriptions.
- Acceptance criteria:
  - backend summary endpoint returns user-scoped aggregates only
  - summary includes month spend, active-subscription monthly projection, spend-by-category aggregation, recent expenses, and upcoming renewals
  - recent expenses and renewals ordering follows backend endpoint notes and shared API contract
  - frontend dashboard hookup can be completed later without backend contract drift

## QUAL-001 - MVP hardening pass

- Status: `In Progress`
- Suggested branch: `chore/backend-hardening-pass`
- Depends on: `AUTH-001`, `AUTH-002`, `CAT-001`, `EXP-001`, `SUB-001`, `DASH-001`
- Goal: tighten the MVP after the main flows exist.
- Backend-first note: harden backend validation and persistence safeguards now while frontend shell/screens remain deferred.
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
