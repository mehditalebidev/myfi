# Worklog

Use this file for short dated notes that help the next agent understand what was
started, merged, blocked, or learned.

## Entry Format

```text
YYYY-MM-DD | Item ID | Status | Branch/PR | Note
```

Keep entries brief and append new lines near the top of the active log section.

## Current Log

- 2026-03-15 | QUAL-001 | in_progress | `feat/category-crud-backend` | Added backend validation and EF column-length guardrails for expense/subscription text fields with a new migration and validator test coverage to prevent runtime DB exceptions on oversized payloads.
- 2026-03-15 | QUAL-001 | in_progress | `feat/category-crud-backend` | Started backend-first hardening pass so validation and persistence safeguards can tighten before frontend shell work resumes.
- 2026-03-15 | DASH-001 | in_progress | `feat/category-crud-backend` | Added backend dashboard summary endpoint with user-scoped monthly spend and subscription projections, spend-by-category aggregation, recent expenses/upcoming renewals lists, plus unit and integration coverage.
- 2026-03-15 | DASH-001 | in_progress | `feat/category-crud-backend` | Started backend-first dashboard summary work so backend MVP delivery can continue while frontend dashboard screens remain deferred.
- 2026-03-15 | SUB-001 | in_progress | `feat/category-crud-backend` | Added backend subscription CRUD endpoints, query support, EF persistence, migration, and unit/integration coverage while frontend work remains deferred.
- 2026-03-15 | SUB-001 | in_progress | `feat/category-crud-backend` | Started backend-first subscription CRUD so backend delivery can continue while frontend scaffold and auth shell work remain deferred.
- 2026-03-15 | EXP-001 | in_progress | `feat/category-crud-backend` | Added backend expense CRUD endpoints, query support, EF persistence, migration, and unit/integration coverage while frontend work remains deferred.
- 2026-03-15 | EXP-001 | in_progress | `feat/category-crud-backend` | Started backend-first expense CRUD so backend delivery can continue while frontend scaffold and auth shell work remain deferred.
- 2026-03-15 | CAT-001 | in_progress | `feat/category-crud-backend` | Added backend category CRUD endpoints, EF persistence, migration, and unit/integration coverage while frontend work remains deferred.
- 2026-03-15 | CAT-001 | in_progress | `feat/category-crud-backend` | Started backend-first category CRUD so backend slices can keep moving while frontend scaffold and auth shell work remain deferred.
- 2026-03-15 | AUTH-001 | in_progress | `feat/backend-unit-tests` | Added `MyFi.Api.UnitTests` with coverage for auth domain behavior, validators, handlers, result helpers, ProblemDetails mapping, and security helpers.
- 2026-03-15 | AUTH-001 | in_progress | `feat/api-versioning-scalar` | Synced shared and coordination docs with the current local auth bootstrap, versioned API routes, and backend-first scaffold state.
- 2026-03-15 | AUTH-001 | in_progress | `feat/api-versioning-scalar` | Added Scalar API docs and introduced `v1` route-based API versioning for the auth and users endpoints.
- 2026-03-14 | AUTH-001 | in_progress | `feat/api-versioning-scalar` | Replaced the in-memory API test host with WebApplicationFactory plus a seeded PostgreSQL Testcontainers setup for integration tests.
- 2026-03-14 | AUTH-001 | in_progress | `feat/api-versioning-scalar` | Reorganized the users slice into folders and switched auth handling to MediatR, FluentValidation, result objects, and ProblemDetails responses.
- 2026-03-14 | AUTH-001 | in_progress | `feat/api-versioning-scalar` | Reworked the backend toward a simple vertical-slice API and replaced the hello sample with local signup, login, and `users/me` auth foundations.
- 2026-03-14 | SETUP-003 | in_progress | `feat/api-versioning-scalar` | Started backend-first scaffold with the .NET solution, PostgreSQL wiring, Docker Compose setup, and a validated hello endpoint slice.
- 2026-03-14 | SETUP-002 | done | PR #1 | Added workspace entrypoints, local docs/src placeholders, and `CONTRIBUTING.md`.
- 2026-03-14 | SETUP-001 | done | `main` | Added the root `README.md` plus initial `frontend/` and `backend/` directories.

## Logging Rules

- Add an entry when a story moves to `In Progress`, `In Review`, `Blocked`, or `Done`.
- Reference the item ID from `BOARD.md` or the corresponding feature/bug entry.
- Mention the branch name or PR number whenever available.
- Do not replace the board with the worklog; the board shows current state, while
  the worklog shows the timeline.
