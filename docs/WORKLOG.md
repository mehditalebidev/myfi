# Worklog

Use this file for short dated notes that help the next agent understand what was
started, merged, blocked, or learned.

## Entry Format

```text
YYYY-MM-DD | Item ID | Status | Branch/PR | Note
```

Keep entries brief and append new lines near the top of the active log section.

## Current Log

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
