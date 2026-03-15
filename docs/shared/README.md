# Project Docs

This directory contains the planning and handoff material for the Personal Finance & Subscription Tracker.

## Ownership Split

- `docs/shared/` contains information both frontend and backend work depend on.
- `docs/frontend/` contains frontend-specific architecture, page, and implementation notes.
- `docs/backend/` contains backend-specific architecture, schema, security, and endpoint notes.
- `docs/coordination/` contains planner-to-implementer workflow, board state, and feature definitions.

## Handoff Package For Frontend Developer

Share these folders/files with the frontend developer:

- `docs/shared/product-scope.md`
- `docs/shared/domain-model.md`
- `docs/shared/auth-flow.md`
- `docs/shared/api-contract.md`
- `docs/shared/repo-structure.md`
- `docs/shared/frontend-handoff.md`
- `docs/shared/implementation-roadmap.md`
- `docs/frontend/`

Do not block frontend work on backend implementation details unless the API contract changes.

## Recommended Reading Order

1. `docs/shared/product-scope.md`
2. `docs/shared/domain-model.md`
3. `docs/shared/auth-flow.md`
4. `docs/shared/api-contract.md`
5. `docs/shared/repo-structure.md`
6. `docs/coordination/README.md`
7. `docs/coordination/BOARD.md`
8. `docs/frontend/` or `docs/backend/` depending on ownership

## Current Planning Decisions

- App type: full-stack web application
- Frontend: React + TypeScript + Vite
- Backend: ASP.NET Core Web API
- Database: PostgreSQL
- Auth style: SPA uses backend-issued JWTs; the current bootstrap is local email/password with room for future providers
- Initial deployment priority: local-first development
- Product priority: balanced full-stack portfolio project
- MVP focus: auth, categories, expenses, subscriptions, dashboard
- Later phases: budgets, reminders, CSV import/export, broader reporting
