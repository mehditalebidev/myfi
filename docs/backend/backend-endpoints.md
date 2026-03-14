# Backend Endpoints

This document complements the shared API contract with backend implementation notes.

## Auth

### `POST /api/auth/google/start`

Responsibilities:

- create or return provider authorization URL
- preserve anti-forgery state if needed
- start backend-managed OAuth flow

### `GET /api/auth/google/callback`

Responsibilities:

- validate callback state
- exchange code with Google
- read provider profile
- create or link local user
- issue access and refresh tokens
- redirect to frontend callback route

### `POST /api/auth/refresh`

Responsibilities:

- validate refresh token
- verify stored token hash
- rotate refresh token
- issue new access token

### `POST /api/auth/logout`

Responsibilities:

- revoke refresh token
- invalidate future refresh for that token chain if desired

## Users

### `GET /api/users/me`

Responsibilities:

- read authenticated local user
- return minimal profile data

## Categories

### `GET /api/categories`

- return all categories for current user
- default ordering by name

### `POST /api/categories`

- validate name required and unique per user

### `PUT /api/categories/{id}`

- validate resource belongs to current user

### `DELETE /api/categories/{id}`

- define behavior for referenced expenses/subscriptions before implementation
- recommended early behavior: allow deletion only if not referenced, or set references nullable after confirmation

## Expenses

### `GET /api/expenses`

Query support:

- pagination
- search by title and note
- filter by category
- filter by date range
- sorting by date or amount

Implementation notes:

- always scope by `user_id`
- project to list DTOs efficiently

### `POST /api/expenses`

Validation notes:

- title required
- amount > 0
- valid category if supplied and belongs to current user
- valid date

### `GET /api/expenses/{id}` / `PUT /api/expenses/{id}` / `DELETE /api/expenses/{id}`

- return `404` for missing or foreign resource ids

## Subscriptions

### `GET /api/subscriptions`

Recommended support:

- pagination
- search by name
- filter by active status
- sorting by renewal date or amount

### `POST /api/subscriptions`

Validation notes:

- name required
- amount > 0
- billing cycle in allowed set
- renewal date required
- valid category if supplied and belongs to current user

### `GET /api/subscriptions/{id}` / `PUT /api/subscriptions/{id}` / `DELETE /api/subscriptions/{id}`

- return `404` for missing or foreign resource ids

## Dashboard

### `GET /api/dashboard/summary`

Responsibilities:

- calculate current month spend
- calculate active subscription monthly cost projection
- aggregate spend by category for current month
- return recent expenses ordered by date descending
- return upcoming renewals ordered by renewal date ascending

Implementation note:

- keep this as query projection logic; do not create stored snapshot tables for phase 1
