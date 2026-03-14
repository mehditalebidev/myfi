# Frontend Architecture

## Tech Stack

- React
- TypeScript
- Vite
- TanStack Query
- React Hook Form
- Zod
- Tailwind CSS

Optional later:

- shadcn/ui for composed primitives
- Recharts for dashboard visualizations

## Architecture Goals

- keep data fetching predictable and feature-based
- keep forms easy to validate and reuse
- separate route pages from reusable feature components
- minimize global state to auth and app-level concerns

## App Composition

- `app/providers/` for React Query provider, router provider, and auth bootstrap
- `app/router/` for route definitions and protected route handling
- `features/*` for domain-specific queries, mutations, components, and forms
- `components/*` for shared UI building blocks
- `api/` for HTTP client, token refresh, and typed resource modules

## State Strategy

- server state: TanStack Query
- form state: React Hook Form
- client validation: Zod
- minimal app state: auth status, active user, UI preferences if needed

## API Layer Strategy

- one base HTTP client with auth header injection
- centralized token refresh handling
- resource modules such as `expensesApi`, `categoriesApi`, `subscriptionsApi`, `dashboardApi`
- keep DTOs aligned with `docs/shared/api-contract.md`

## Feature Module Recommendation

Each major feature should contain:

- query hooks
- mutation hooks
- form schema
- form component
- list/table view component
- route-level page adapter if needed

## UI Priorities

- fast entry for expenses
- clear monthly summary
- consistent empty and loading states
- good table/list readability on desktop and mobile

## Auth UX Notes

- unauthenticated users see login page only
- callback page handles backend redirect completion and bootstraps session state
- protected routes should redirect unauthenticated users to login

## Keep Out Of Frontend For Now

- backend-only business rules
- Google provider details beyond invoking the auth flow
- advanced client-side caching beyond what TanStack Query already provides
