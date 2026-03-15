# Product Scope

## Project Summary

Build a practical personal finance application that helps a single user track expenses, subscriptions, and monthly spending patterns without becoming a full accounting platform.

## Product Goals

- Track day-to-day expenses with categories and notes
- Track recurring subscriptions and upcoming renewals
- Show a clear monthly dashboard
- Practice real full-stack architecture with authentication, data modeling, validation, and reporting
- Keep scope small enough to finish as a learning project

## MVP Scope

The first usable version includes:

- local email/password signup and login
- authenticated current-user profile lookup
- category CRUD
- expense CRUD
- subscription CRUD
- dashboard summary
- responsive web UI for core pages

## Explicitly Deferred From MVP

- budgets
- reminders and notifications
- CSV import/export
- advanced reports
- advanced recurring schedule rules
- Google OAuth and external identity linking
- multiple auth providers beyond local email/password
- shared household features
- bank integrations

## Phase Breakdown

### Phase 1 - MVP

- Authentication with local email/password and backend-issued JWTs
- Categories, expenses, subscriptions
- Dashboard summary
- Validation and basic error handling

### Phase 2 - Domain Expansion

- Budgets
- Simple recurring expense logic
- Search, filtering, sorting, pagination
- Charts and richer reports

### Phase 3 - Utility and Polish

- In-app reminders
- CSV import/export
- Testing depth
- deployment setup
- groundwork for additional auth providers

## Non-Goals

- Accounting-grade ledger features
- tax tools
- investment tracking
- invoicing
- household collaboration in v1

## Success Criteria

- A user can authenticate and only access their own data
- A user can manage categories, expenses, and subscriptions reliably
- The dashboard is useful enough to review monthly activity quickly
- The architecture clearly demonstrates frontend and backend skills
- The project remains small enough to complete and extend later
