# Backend Endpoints

This document complements the shared API contract with backend implementation notes.

Implemented now: the auth and `users/me` routes below. The remaining sections describe the planned next backend slices.

## Auth

### `POST /api/v1/auth/signup`

Responsibilities:

- validate the signup payload
- dispatch through MediatR
- ensure email uniqueness
- hash the password
- create the local user
- issue an access token
- return `ProblemDetails` on conflict

### `POST /api/v1/auth/login`

Responsibilities:

- validate the login payload
- dispatch through MediatR
- verify the stored password hash
- issue an access token
- return `ProblemDetails` on invalid credentials

## Users

### `GET /api/v1/users/me`

Versioning notes:

- current HTTP route version is `v1`
- API version metadata is reported in response headers
- development docs are exposed through Scalar at `/docs`

Responsibilities:

- read authenticated local user
- dispatch through MediatR
- return minimal profile data

## Categories

### `GET /api/v1/categories`

- return all categories for current user
- default ordering by name

### `POST /api/v1/categories`

- validate name required and unique per user

### `PUT /api/v1/categories/{id}`

- validate resource belongs to current user

### `DELETE /api/v1/categories/{id}`

- define behavior for referenced expenses/subscriptions before implementation
- recommended early behavior: allow deletion only if not referenced, or set references nullable after confirmation

## Expenses

### `GET /api/v1/expenses`

Query support:

- pagination
- search by title and note
- filter by category
- filter by date range
- sorting by date or amount

Implementation notes:

- always scope by `user_id`
- project to list DTOs efficiently

### `POST /api/v1/expenses`

Validation notes:

- title required
- amount > 0
- valid category if supplied and belongs to current user
- valid date

### `GET /api/v1/expenses/{id}` / `PUT /api/v1/expenses/{id}` / `DELETE /api/v1/expenses/{id}`

- return `404` for missing or foreign resource ids

## Subscriptions

### `GET /api/v1/subscriptions`

Recommended support:

- pagination
- search by name
- filter by active status
- sorting by renewal date or amount

### `POST /api/v1/subscriptions`

Validation notes:

- name required
- amount > 0
- billing cycle in allowed set
- renewal date required
- valid category if supplied and belongs to current user

### `GET /api/v1/subscriptions/{id}` / `PUT /api/v1/subscriptions/{id}` / `DELETE /api/v1/subscriptions/{id}`

- return `404` for missing or foreign resource ids

## Dashboard

### `GET /api/v1/dashboard/summary`

Responsibilities:

- calculate current month spend
- calculate active subscription monthly cost projection
- aggregate spend by category for current month
- return recent expenses ordered by date descending
- return upcoming renewals ordered by renewal date ascending

Implementation note:

- keep this as query projection logic; do not create stored snapshot tables for phase 1
