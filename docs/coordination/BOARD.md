# Coordination Board

This board is the current source of truth for what is ready, active, blocked,
or complete.

Update it whenever work changes status.

## Ready

| ID | Title | Notes |
| --- | --- | --- |

## Backlog

| ID | Title | Notes |
| --- | --- | --- |
| AUTH-002 | Build frontend auth shell | Promote to `Ready` after `SETUP-003` and `AUTH-001` land. |
| CAT-001 | Category CRUD vertical slice | Depends on backend and frontend scaffolds plus auth foundation. |
| EXP-001 | Expense CRUD vertical slice | Depends on category data shape and authenticated app shell. |
| SUB-001 | Subscription CRUD vertical slice | Depends on category data shape and authenticated app shell. |
| DASH-001 | Dashboard summary vertical slice | Depends on expenses and subscriptions data access. |
| QUAL-001 | Hardening pass for validation and states | Run after the core MVP flows exist end to end. |
| BUD-001 | Budgets phase kickoff | Deferred from MVP. |
| REP-001 | Rich reporting and charts | Deferred from MVP. |
| REM-001 | In-app reminders | Deferred from MVP. |
| CSV-001 | CSV import/export | Deferred from MVP. |

## In Progress

| ID | Title | Notes |
| --- | --- | --- |
| SETUP-003 | Initialize frontend and backend project scaffolds | Backend-first implementation started per user direction. Frontend scaffold still pending. |
| AUTH-001 | Build backend auth foundation | User-directed local email/password auth is in progress with organized vertical slices, MediatR, FluentValidation, ProblemDetails, seeded PostgreSQL integration tests, Scalar docs, and API versioning. |

## In Review

No coordination-tracked product item is currently in review.

## Blocked

No blocked items yet.

## Done

| ID | Title | Outcome |
| --- | --- | --- |
| SETUP-001 | Add root README and initial app directories | Landed on `main` with the initial monorepo placeholders. |
| SETUP-002 | Add workspace entrypoints and contributing guide | Landed on `main` with `frontend/README.md`, `backend/README.md`, local docs/src placeholders, and `CONTRIBUTING.md`. |

## Board Maintenance Rules

- Keep only actionable items in `Ready`.
- Move an item to `In Progress` as soon as an implementer starts a branch for it.
- Include the PR link in `In Review` once a branch is pushed and reviewed.
- Move items to `Done` only after a human merges the PR.
