# MVP API Contract

This document defines the phase 1 API surface the frontend can build against.

## API Conventions

- Base route prefix: `/api/v{version}`
- Current implemented version: `v1`
- JSON request and response bodies
- Authenticated routes require bearer token
- All timestamps are ISO 8601 strings
- Money uses decimal-compatible JSON numbers
- List endpoints support pagination when relevant

## Error Shape

All API errors should use ProblemDetails-style JSON.

Non-validation example:

```json
{
  "type": "https://httpstatuses.com/404",
  "title": "Expense was not found.",
  "status": 404,
  "detail": "The requested expense does not exist.",
  "code": "resource_not_found"
}
```

Validation example:

```json
{
  "type": "https://httpstatuses.com/400",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "detail": "See the errors property for details.",
  "code": "validation_failed",
  "errors": {
    "title": ["Title is required."],
    "amount": ["Amount must be greater than zero."]
  }
}
```

## Authentication

### `POST /api/v1/auth/signup`

Creates a local user and returns an access token.

Request:

```json
{
  "email": "user@example.com",
  "displayName": "Jane Doe",
  "password": "Password123!"
}
```

Response:

- `200` with auth state

Example:

```json
{
  "accessToken": "string",
  "expiresAt": "2026-03-14T12:00:00Z",
  "user": {
    "id": "uuid",
    "email": "user@example.com",
    "displayName": "Jane Doe"
  }
}
```

### `POST /api/v1/auth/login`

Logs an existing user in and returns an access token.

Request:

```json
{
  "email": "user@example.com",
  "password": "Password123!"
}
```

Expected result:

- access token issued
- authenticated user returned

### `GET /api/v1/users/me`

Returns the authenticated user.

Response:

```json
{
  "id": "uuid",
  "email": "user@example.com",
  "displayName": "Jane Doe"
}
```

## Categories

### Category Shape

```json
{
  "id": "uuid",
  "name": "Groceries",
  "color": "#3b82f6",
  "icon": "shopping-cart",
  "createdAt": "2026-03-14T12:00:00Z",
  "updatedAt": "2026-03-14T12:00:00Z"
}
```

### `GET /api/categories`

Response:

```json
[
  {
    "id": "uuid",
    "name": "Groceries",
    "color": "#3b82f6",
    "icon": "shopping-cart",
    "createdAt": "2026-03-14T12:00:00Z",
    "updatedAt": "2026-03-14T12:00:00Z"
  }
]
```

### `POST /api/categories`

Request:

```json
{
  "name": "Groceries",
  "color": "#3b82f6",
  "icon": "shopping-cart"
}
```

Response: created category

### `PUT /api/categories/{id}`

Request matches create payload.

### `DELETE /api/categories/{id}`

Response: `204 No Content`

## Expenses

### Expense Shape

```json
{
  "id": "uuid",
  "title": "Supermarket",
  "amount": 84.50,
  "expenseDate": "2026-03-12",
  "categoryId": "uuid",
  "categoryName": "Groceries",
  "paymentMethod": "card",
  "note": "Weekly shopping",
  "isRecurring": false,
  "createdAt": "2026-03-14T12:00:00Z",
  "updatedAt": "2026-03-14T12:00:00Z"
}
```

### `GET /api/expenses`

Query params:

- `page`
- `pageSize`
- `search`
- `categoryId`
- `dateFrom`
- `dateTo`
- `sortBy` with values like `expenseDate` or `amount`
- `sortDir` with values `asc` or `desc`

Response:

```json
{
  "items": [],
  "page": 1,
  "pageSize": 20,
  "totalCount": 0,
  "totalPages": 0
}
```

### `POST /api/expenses`

Request:

```json
{
  "title": "Supermarket",
  "amount": 84.50,
  "expenseDate": "2026-03-12",
  "categoryId": "uuid",
  "paymentMethod": "card",
  "note": "Weekly shopping",
  "isRecurring": false
}
```

Response: created expense

### `GET /api/expenses/{id}`

Response: expense

### `PUT /api/expenses/{id}`

Request matches create payload.

### `DELETE /api/expenses/{id}`

Response: `204 No Content`

## Subscriptions

### Subscription Shape

```json
{
  "id": "uuid",
  "name": "Netflix",
  "amount": 15.99,
  "billingCycle": "monthly",
  "renewalDate": "2026-03-28",
  "categoryId": "uuid",
  "categoryName": "Entertainment",
  "isActive": true,
  "reminderDaysBefore": 3,
  "createdAt": "2026-03-14T12:00:00Z",
  "updatedAt": "2026-03-14T12:00:00Z"
}
```

### `GET /api/subscriptions`

Response:

```json
{
  "items": [],
  "page": 1,
  "pageSize": 20,
  "totalCount": 0,
  "totalPages": 0
}
```

### `POST /api/subscriptions`

Request:

```json
{
  "name": "Netflix",
  "amount": 15.99,
  "billingCycle": "monthly",
  "renewalDate": "2026-03-28",
  "categoryId": "uuid",
  "isActive": true,
  "reminderDaysBefore": 3
}
```

Response: created subscription

### `GET /api/subscriptions/{id}`

Response: subscription

### `PUT /api/subscriptions/{id}`

Request matches create payload.

### `DELETE /api/subscriptions/{id}`

Response: `204 No Content`

## Dashboard

### `GET /api/dashboard/summary`

Response:

```json
{
  "month": 3,
  "year": 2026,
  "monthlySpend": 1284.10,
  "monthlySubscriptionCost": 52.98,
  "spendByCategory": [
    {
      "categoryId": "uuid",
      "categoryName": "Groceries",
      "amount": 420.00
    }
  ],
  "recentExpenses": [],
  "upcomingRenewals": []
}
```

## Frontend Integration Notes

- Build query hooks around resource groups: auth, categories, expenses, subscriptions, dashboard.
- Treat field names here as contract names unless changed jointly.
- The frontend can safely mock against this document before the backend exists.
