# AGENTS.md

## Purpose
- This file guides agentic coding agents working in this repository.
- The repo is currently docs-first; do not pretend missing apps, scripts, or configs already exist.
- Follow the planning docs first, then align implementation to them.

## Current State
- The repository currently contains planning and handoff docs under `docs/`.
- Initial `frontend/` and `backend/` workspace folders exist with local `README.md`, `docs/`, and `src/` placeholders.
- No runnable frontend/backend projects or scripts are checked in yet.
- Command examples below are the intended defaults once the scaffold exists.

## Extra Instruction Files
- Checked and not found: root `AGENTS.md` before this file, `.cursorrules`, `.cursor/rules/`, `.github/copilot-instructions.md`.
- If any of those files are added later, merge their guidance with this file instead of ignoring them.

## Read First
- `docs/shared/README.md`, `docs/shared/product-scope.md`, `docs/shared/domain-model.md`
- `docs/shared/auth-flow.md`, `docs/shared/api-contract.md`, `docs/shared/repo-structure.md`
- `docs/frontend/frontend-architecture.md`, `docs/frontend/ui-and-components.md`
- `docs/backend/backend-architecture.md`, `docs/backend/backend-endpoints.md`, `docs/backend/security-and-auth.md`
- For frontend work, also read `frontend/README.md` and then `frontend/docs/` if present.
- For backend work, also read `backend/README.md` and then `backend/docs/` if present.

## Repo Shape To Preserve
- Top level target: `docs/`, `frontend/`, `backend/`, `.env.example`, `docker-compose.yml`.
- Frontend stack target: React, TypeScript, Vite, TanStack Query, React Hook Form, Zod, Tailwind CSS.
- Backend stack target: ASP.NET Core Web API, EF Core, PostgreSQL, FluentValidation, JWT auth.

## Command Policy
- Prefer the narrowest command that proves your change.
- For docs-only changes, no build is required.
- For frontend-only work, run frontend checks only.
- For backend-only work, prefer targeted `dotnet test` filters before full-solution runs.
- If the scaffold does not exist yet, say the command is planned but not currently runnable.

## Frontend Commands
- Run from `frontend/` once it exists.
- Install: `npm install`
- Dev server: `npm run dev`
- Build/lint/type-check: `npm run build`, `npm run lint`, `npm run typecheck`
- Run all tests: `npm run test`
- Run one test file: `npm run test -- src/features/expenses/components/ExpenseForm.test.tsx`
- Run tests matching a name: `npm run test -- --testNamePattern="submits valid expense"`
- Assume Vitest-style targeting unless the future scaffold defines another runner.

## Backend Commands
- Run from `backend/` once it exists.
- Restore/build: `dotnet restore`, `dotnet build MyFi.sln`
- Run all tests: `dotnet test MyFi.sln`
- Run one test project: `dotnet test tests/MyFi.Application.Tests/MyFi.Application.Tests.csproj`
- Run one test class: `dotnet test tests/MyFi.Application.Tests/MyFi.Application.Tests.csproj --filter "FullyQualifiedName~CreateExpenseCommandHandlerTests"`
- Run one test method: `dotnet test tests/MyFi.Application.Tests/MyFi.Application.Tests.csproj --filter "Name~Should_Create_Expense_When_Request_Is_Valid"`

## Infra Commands
- Run from repo root once infra files exist.
- Start local services: `docker-compose up --build`
- Stop local services: `docker-compose down`

## Architecture Rules
- Preserve the planned boundary: `API -> Application -> Domain -> Infrastructure`.
- Keep controllers and route handlers thin.
- Keep business rules in Application or Domain, not transport layers.
- Avoid over-abstracting repositories when direct framework usage is clearer.
- Keep user scoping explicit in every query and mutation.
- Do not silently change the planned architecture direction.

## Shared Code Style
- Keep code simple, explicit, and close to the documented architecture.
- Prefer small focused modules over large multi-purpose files; favor readability over cleverness.
- Remove dead code and unused imports.
- Use ASCII unless the file already requires Unicode; add comments only when they clarify non-obvious behavior.

## Import Rules
- Group imports consistently: external first, then internal.
- Keep import lists stable and clean.
- Prefer project-defined aliases only after they are actually configured.
- Do not add new libraries casually; use the planned stack first.

## Frontend Structure Rules
- Shared UI belongs in `components/`.
- Feature-specific code belongs in `features/*`.
- Route wiring belongs in `app/router/`.
- Providers and app bootstrap belong in `app/providers/`.
- API clients and token handling belong in `api/`.
- Do not let page files collect API logic, schema logic, and heavy UI logic together.

## Backend Structure Rules
- Domain must not depend on API or infrastructure concerns.
- Application can depend on Domain.
- Infrastructure can depend on Application and Domain.
- Keep EF Core and provider integrations out of Domain.
- Keep validation in the Application layer.

## Formatting
- Use the repository formatter once it is configured.
- Until then, follow conventional defaults for the language.
- Keep lines readable rather than compressed.
- Prefer one statement per line when clarity matters.
- Do not hand-format files inconsistently.

## TypeScript Rules
- Prefer explicit types and DTOs over `any`.
- Prefer `unknown` over `any` for untrusted input.
- Keep API types aligned with `docs/shared/api-contract.md`.
- Use Zod schemas at form and request boundaries.
- Use narrow unions for finite UI or API states.
- Keep server state in TanStack Query, not ad hoc globals.
- Keep form state in React Hook Form.

## C# Rules
- Use strong request and response DTOs.
- Use `decimal` for money values; never use floating point for finance.
- Keep Domain entities framework-agnostic.
- Use nullable reference types consistently if enabled.
- Keep current-user scoping explicit in handlers and queries.

## Naming Conventions
- Prefer descriptive domain names over abbreviations.
- Name by business meaning, not UI position.
- Follow the project language: user, category, expense, subscription, dashboard.
- React components use PascalCase; hooks use `useX` names.
- TypeScript variables/functions use camelCase; types/interfaces use PascalCase.
- C# types/methods/properties use PascalCase; locals/parameters use camelCase.

## Error Handling And Validation
- Validate input at system boundaries.
- Frontend forms should validate client-side with Zod before submit.
- Backend requests should validate in the Application layer with FluentValidation.
- Return consistent error responses matching `docs/shared/api-contract.md`.
- Return `404` for missing or foreign-owned resources where the contract expects it.
- Never leak secrets, provider secrets, raw refresh tokens, or internal exception details.
- Every read and write must be scoped by authenticated user id.

## Auth And Security
- Backend owns Google OAuth details.
- Frontend should treat auth as backend-issued token auth.
- Keep JWT claims minimal and app-specific.
- Refresh tokens should be rotated and stored securely; never trust client-provided ownership data.
- Log failures without logging sensitive token values.

## Testing Expectations
- Test behavior, not implementation trivia.
- Add or update a test for bug fixes when practical.
- Prefer unit tests for validation and application logic.
- Prefer integration tests for endpoint wiring and auth-sensitive flows.
- Keep test data realistic for money, dates, categories, and subscriptions.

## Documentation Rules
- `docs/shared/api-contract.md` is the API contract source of truth.
- Do not invent incompatible field names in mocks or DTOs.
- Update docs when changing endpoint shapes, auth flow, or ownership boundaries.
- Keep planning docs and implementation aligned while the project is still young.

## Agent Working Style
- Read the relevant docs before large edits.
- Prefer targeted edits over broad rewrites.
- Be explicit when something is planned versus implemented.
- If code and docs conflict, resolve the conflict deliberately and update both when needed.
