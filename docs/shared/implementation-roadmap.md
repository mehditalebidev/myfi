# Near-Term Implementation Roadmap

This roadmap reflects the current checked-in state after the backend-first bootstrap work.

## Current Foundation Status

- repository docs, coordination files, and workspace entrypoints are in place
- backend scaffold exists under `backend/` with PostgreSQL wiring, EF Core migrations, local signup/login, `users/me`, ProblemDetails responses, API versioning, Scalar docs, and integration tests
- frontend workspace still needs its real React + Vite scaffold under `frontend/`

## Next Execution Sequence

1. Finish `SETUP-003` by initializing the frontend scaffold under `frontend/`.
2. Continue `AUTH-001` by keeping the backend auth bootstrap aligned with the shared contract and security docs.
3. Implement `AUTH-002` so the frontend can log in, hold JWT session state, and protect authenticated routes.
4. Build category CRUD as the first full authenticated data slice.
5. Follow with expense CRUD, subscription CRUD, and the dashboard summary slice.
6. Run a hardening pass once the main MVP flows exist end to end.

## Active Constraints

- Keep the backend on the current vertical-slice path unless the architecture docs are deliberately changed.
- Treat `docs/shared/api-contract.md` as the source of truth for frontend/backend integration.
- Do not move into budgets, reminders, or CSV work until the MVP flows are stable.
