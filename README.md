# MyFi

MyFi is a personal finance and subscription tracker planned as a full-stack portfolio project.

## Current State

- This repository is in planning and scaffold setup stage.
- The implementation docs live under `docs/`.
- The `frontend/` and `backend/` directories now contain initial workspace placeholders.
- No frontend or backend projects have been initialized yet.

## Planned Stack

- Frontend: React, TypeScript, Vite, TanStack Query, React Hook Form, Zod, Tailwind CSS
- Backend: ASP.NET Core Web API, EF Core, PostgreSQL, FluentValidation, JWT auth

## Repository Layout

```text
myfi/
  docs/
    shared/
    coordination/
    frontend/
    backend/
    WORKLOG.md
  frontend/
    README.md
    docs/
    src/
  backend/
    README.md
    docs/
    src/
  AGENTS.md
  CONTRIBUTING.md
  README.md
```

## Docs To Read First

1. `docs/shared/README.md`
2. `docs/shared/product-scope.md`
3. `docs/shared/domain-model.md`
4. `docs/shared/auth-flow.md`
5. `docs/shared/api-contract.md`
6. `docs/shared/repo-structure.md`
7. `docs/coordination/README.md`
8. `docs/coordination/BOARD.md`
9. `docs/WORKLOG.md`
10. `CONTRIBUTING.md`
11. `frontend/README.md` or `backend/README.md` depending on the area you are changing

## Notes

- Keep the frontend and backend in this monorepo for now.
- Initialize app scaffolds later inside `frontend/` and `backend/`.
- Update the shared docs first if the API contract or architecture changes.
- Use the local `frontend/README.md` and `backend/README.md` files as area-specific starting points for future work.
- Use `docs/coordination/` and `docs/WORKLOG.md` to manage planner-to-implementer handoff in markdown.
