# AGENTS.md

## Purpose
- This file guides agentic coding agents working in this repository.
- The repo is currently docs-first; do not pretend missing apps, scripts, or configs already exist.
- Follow the planning docs first, then align implementation to them.

## Current State
- The repository currently contains planning and handoff docs under `docs/`.
- The `frontend/` workspace is still a placeholder with local `README.md`, `docs/`, and `src/` entrypoints.
- The `backend/` workspace now contains a runnable ASP.NET Core API, local development docs, Docker Compose wiring, EF Core migrations, and integration tests.
- Command examples below should match the checked-in backend reality; call out frontend commands as planned until the frontend scaffold exists.

## Extra Instruction Files
- Checked and not found: root `AGENTS.md` before this file, `.cursorrules`, `.cursor/rules/`, `.github/copilot-instructions.md`.
- If any of those files are added later, merge their guidance with this file instead of ignoring them.

## Read First
- `docs/shared/README.md`, `docs/shared/product-scope.md`, `docs/shared/domain-model.md`
- `docs/shared/auth-flow.md`, `docs/shared/api-contract.md`, `docs/shared/repo-structure.md`
- `docs/coordination/README.md`, `docs/coordination/OPERATING_MODEL.md`, `docs/coordination/BOARD.md`
- `docs/WORKLOG.md` before continuing an existing story or bug
- `docs/frontend/frontend-architecture.md`, `docs/frontend/ui-and-components.md`
- `docs/backend/backend-architecture.md`, `docs/backend/backend-endpoints.md`, `docs/backend/security-and-auth.md`
- For frontend work, also read `frontend/README.md` and then `frontend/docs/` if present.
- For backend work, also read `backend/README.md` and then `backend/docs/` if present.

## Coordination Workflow
- Planner/product-owner agents should create and refine work in `docs/coordination/` and should not implement product code in that role.
- Implementer agents should pick a `Ready` item from `docs/coordination/BOARD.md` unless the user explicitly overrides that rule.
- Before starting a new feature, implementer agents should review items already in `In Review` and check whether their PRs were merged so `docs/coordination/BOARD.md` and `docs/WORKLOG.md` can be updated.
- Implementer agents should update `docs/coordination/BOARD.md` and `docs/WORKLOG.md` as work moves through `In Progress`, `In Review`, `Blocked`, and `Done`.
- Humans merge pull requests; agents should open PRs to `main` but not merge unless explicitly instructed.

## Repo Shape To Preserve
- Top level target: `docs/`, `frontend/`, `backend/`, `.env.example`, `docker-compose.yml`.
- Frontend stack target: React, TypeScript, Vite, TanStack Query, React Hook Form, Zod, Tailwind CSS.
- Backend stack target: ASP.NET Core Web API, EF Core, PostgreSQL, FluentValidation, JWT auth.

## Required Git Workflow
- For any feature, story, fix, or meaningful repo task, do not work directly on `main`.
- Before starting unrelated new work, return to the latest `main` and create a focused branch from there.
- Preferred branch prefixes: `feat/`, `fix/`, `docs/`, `chore/`.
- Only continue on the current branch when the new changes are part of that same ongoing work.
- Commit the work on that branch, push it to GitHub, and open a pull request targeting `main`.
- Treat branch -> commit -> push -> PR as the default workflow for agents in this repository.
- After opening a PR, switch back to `main` locally and delete the local feature branch copy.
- Keep GitHub configured to automatically delete the remote feature branch after the PR is merged.
- Keep pull requests scoped to one task or feature slice, and resolve merge conflicts on the branch before merge.
- Use `CONTRIBUTING.md` for the human-friendly explanation of the same workflow.

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
- Run from `backend/`.
- Restore/build: `dotnet restore`, `dotnet build MyFi.sln`
- Run API: `dotnet run --project src/MyFi.Api/MyFi.Api.csproj`
- Run all tests: `dotnet test MyFi.sln`
- Run one test project: `dotnet test tests/MyFi.Api.IntegrationTests/MyFi.Api.IntegrationTests.csproj`
- Run one test class: `dotnet test tests/MyFi.Api.IntegrationTests/MyFi.Api.IntegrationTests.csproj --filter "FullyQualifiedName~AuthEndpointsTests"`
- Run one test method: `dotnet test tests/MyFi.Api.IntegrationTests/MyFi.Api.IntegrationTests.csproj --filter "Name~Login_And_GetMe_Work_WithSeededUser"`
- Apply migrations locally: `dotnet tool restore && dotnet ef database update --project src/MyFi.Api/MyFi.Api.csproj --startup-project src/MyFi.Api/MyFi.Api.csproj`

## Infra Commands
- Run from repo root once infra files exist.
- Start local services: `docker-compose up --build`
- Stop local services: `docker-compose down`

## Architecture Rules
- Preserve the current backend direction: a simple vertical-slice API inside `MyFi.Api`, with shared cross-cutting code under `Common/`.
- Keep controllers and route handlers thin.
- Keep business rules in slice handlers and feature-local domain types, not transport layers.
- Avoid prematurely splitting the backend into extra class libraries unless the docs are deliberately updated first.
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
- Keep `Features/*` as the primary home for backend business logic.
- Keep shared persistence, security, result, and pipeline helpers in `Common/` only when they are truly cross-cutting.
- Feature-local entities and EF configuration can live inside the same slice when they are specific to that feature.
- Keep validation close to commands and queries and run it through the MediatR pipeline.
- Do not spread one feature across multiple folders unless there is a clear cross-slice reason.

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
- Backend owns auth implementation details.
- The current auth bootstrap is local email/password with backend-issued JWTs.
- If external providers are added later, keep that integration behind backend boundaries.
- Frontend should treat auth as backend-issued token auth.
- Keep JWT claims minimal and app-specific.
- If refresh tokens are added later, rotate and store them securely; never trust client-provided ownership data.
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
