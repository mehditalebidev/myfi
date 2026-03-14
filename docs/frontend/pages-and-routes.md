# Pages And Routes

## Public Routes

### `/login`

Purpose:

- explain the product briefly
- present `Continue with Google`

Primary actions:

- start auth flow

### `/auth/callback`

Purpose:

- complete sign-in after backend redirect
- handle success, loading, or failure state

Primary actions:

- finalize token/session bootstrap
- redirect to dashboard when complete

## Authenticated Routes

### `/`

Redirect to `/dashboard`

### `/dashboard`

Shows:

- monthly total spend
- monthly subscription cost
- spending by category
- recent expenses
- upcoming renewals

### `/expenses`

Shows:

- searchable and sortable expense list
- filter controls
- create action

### `/expenses/new`

Shows create form.

### `/expenses/:id/edit`

Shows edit form.

### `/subscriptions`

Shows:

- subscription list
- status indicator
- renewal dates
- create action

### `/subscriptions/new`

Shows create form.

### `/subscriptions/:id/edit`

Shows edit form.

### `/categories`

Shows:

- category list
- create/edit/delete actions

### `/settings`

Phase 1 can be minimal:

- current user display
- sign out action

## Route Guard Behavior

- public routes redirect authenticated users to `/dashboard`
- authenticated routes redirect unauthenticated users to `/login`
- callback route must show useful error state if auth fails
