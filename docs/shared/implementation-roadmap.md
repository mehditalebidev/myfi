# First-Week Implementation Roadmap

This roadmap is intentionally short and execution-oriented. It assumes work begins only after planning sign-off.

## Week 1 Objective

Finish enough foundation to support authenticated CRUD flows and unblock both frontend and backend work.

## Day 1

- scaffold repository structure
- initialize frontend app and backend solution
- add docs to repo
- configure local PostgreSQL strategy
- create `.env.example`

## Day 2

- set up backend projects and dependency wiring
- configure EF Core database connection
- create `User`, `AuthIdentity`, and `RefreshToken` models
- establish Google auth configuration placeholders
- implement `GET /users/me` path skeleton

## Day 3

- implement auth flow skeleton
- persist local user creation and auth identity linking
- implement token issuance and refresh flow
- wire frontend auth bootstrap and route protection shell

## Day 4

- implement category schema, validation, and CRUD endpoints
- build category screens and category form flow in frontend
- verify authenticated user scoping end to end

## Day 5

- implement expense schema and CRUD endpoints
- build expense list and create/edit form
- add filtering and sorting placeholders to API and UI

## Day 6

- implement subscription schema and CRUD endpoints
- build subscription list and create/edit form
- add upcoming renewals query support

## Day 7

- implement dashboard summary endpoint
- build dashboard widgets and summary cards
- tighten validation, loading states, and empty states
- update docs based on implementation discoveries

## Important Constraint

Do not move into budgets, reminders, or CSV work until the above flows are stable and the contract is still clean.
