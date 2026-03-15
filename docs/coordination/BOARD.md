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
| BUD-001 | Budgets phase kickoff | Deferred from MVP. |
| REP-001 | Rich reporting and charts | Deferred from MVP. |
| REM-001 | In-app reminders | Deferred from MVP. |
| CSV-001 | CSV import/export | Deferred from MVP. |

## In Progress

| ID | Title | Notes |
| --- | --- | --- |
| SETUP-003 | Initialize frontend and backend project scaffolds | Backend-first implementation started per user direction. Frontend scaffold still pending. |
| AUTH-001 | Build backend auth foundation | User-directed local email/password auth is in progress with organized vertical slices, MediatR, FluentValidation, ProblemDetails, seeded PostgreSQL integration tests, unit tests for auth logic, Scalar docs, and API versioning. |
| CAT-001 | Category CRUD vertical slice | Backend-first implementation started while frontend work is deferred; keep API contract and user-scoped CRUD behavior aligned so frontend hookup can land later. |
| EXP-001 | Expense CRUD vertical slice | Backend-first implementation started while frontend work is deferred; keep the documented list/query contract and category ownership rules aligned for later frontend hookup. |
| SUB-001 | Subscription CRUD vertical slice | Backend-first implementation started while frontend work is deferred; keep the documented list/query contract, billing-cycle rules, and category ownership checks aligned for later frontend hookup. |
| DASH-001 | Dashboard summary vertical slice | Backend-first implementation started while frontend work is deferred; keep dashboard summary projections and ordering aligned with the shared contract for later frontend hookup. |
| QUAL-001 | Hardening pass for validation and states | Backend-first hardening started with validator and persistence guardrails to reduce runtime DB failures while frontend remains deferred. |

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
- Before starting a new item, review entries already in `In Review` and move any merged work to `Done`.
- Move an item to `In Progress` as soon as an implementer starts a branch for it.
- Include the PR link in `In Review` once a branch is pushed and reviewed.
- Move items to `Done` only after a human merges the PR.
