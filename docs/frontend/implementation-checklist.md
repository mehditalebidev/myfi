# Frontend Implementation Checklist

This checklist is intentionally ordered to reduce blockers.

## Foundation

- initialize Vite React TypeScript app
- set up Tailwind CSS
- add TanStack Query, React Hook Form, Zod, and router
- create app providers and base layout

## Auth

- create login page
- create callback page
- create auth storage/bootstrap utilities
- create protected route logic
- create current user query hook

## Shared UI

- build layout shell
- build loading, empty, and error states
- build page header and stat card components
- build form field abstractions if helpful

## Categories

- create category API module
- create category query and mutation hooks
- build categories page
- build category create/edit flow

## Expenses

- create expense API module
- create expense query and mutation hooks
- build expenses list page
- build expense create/edit form
- add filter and sort controls

## Subscriptions

- create subscription API module
- create subscription query and mutation hooks
- build subscriptions list page
- build subscription create/edit form

## Dashboard

- create dashboard API module
- build summary cards
- build recent expenses section
- build upcoming renewals section
- add category spending chart placeholder or first chart

## Final Phase 1 Polish

- responsive behavior review
- toast or inline mutation feedback
- confirm destructive actions
- route-level loading and auth failure handling
- verify field names against shared contract docs
