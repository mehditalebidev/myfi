# Frontend Handoff

This document is the handoff brief for the frontend developer.

## Frontend Ownership

The frontend developer is responsible for:

- app shell and routing
- login page and authenticated session handling
- authenticated layout and navigation
- category, expense, subscription, and dashboard pages
- React Query hooks and frontend API module usage
- form UX, client validation, loading states, and responsive behavior

## Inputs The Frontend Can Rely On

- `docs/shared/api-contract.md` is the source of truth for request and response shapes
- `docs/shared/auth-flow.md` defines the intended auth experience
- `docs/shared/domain-model.md` explains business meaning for fields and screens
- `docs/frontend/` contains frontend-specific implementation notes

## Assumptions To Build Against

- auth is token-based from the frontend perspective
- the app has a login page and protected authenticated pages
- current user data comes from `GET /api/v1/users/me`
- categories are reusable across expenses and subscriptions
- dashboard is a read-only summary page for phase 1

## Expectations For Mocking

- mock against `docs/shared/api-contract.md`
- keep mock data realistic for money, dates, categories, and subscriptions
- do not invent incompatible field names
- if a shape seems missing, propose a contract update before implementing around assumptions

## Done Criteria For Frontend Phase 1

- login screen and session bootstrap exist
- authenticated layout works
- categories CRUD screens work against mock or real API
- expenses CRUD screens work against mock or real API
- subscriptions CRUD screens work against mock or real API
- dashboard renders summary cards, recent expenses, and renewals
- responsive layout works on mobile and desktop
- loading, empty, and error states exist for primary flows

## Questions That Require Contract Review

- changing property names or response envelopes
- changing how pagination works
- changing auth payloads or token lifecycle expectations
- changing date or money formatting at the API level
