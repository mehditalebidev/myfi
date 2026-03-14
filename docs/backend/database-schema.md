# Initial Database Schema

This schema covers phase 1 and identifies planned phase 2 additions.

## Database Choice

- PostgreSQL
- UUID primary keys recommended
- timestamps stored in UTC
- money values stored as decimal-compatible numeric type

## Phase 1 Tables

### `users`

Columns:

- `id` uuid primary key
- `email` text not null
- `display_name` text not null
- `created_at` timestamp with time zone not null
- `updated_at` timestamp with time zone not null

Indexes and constraints:

- unique index on `email`

### `auth_identities`

Columns:

- `id` uuid primary key
- `user_id` uuid not null references `users(id)`
- `provider` text not null
- `provider_user_id` text not null
- `provider_email` text null
- `created_at` timestamp with time zone not null
- `last_login_at` timestamp with time zone not null

Indexes and constraints:

- unique index on `provider, provider_user_id`
- index on `user_id`

### `refresh_tokens`

Columns:

- `id` uuid primary key
- `user_id` uuid not null references `users(id)`
- `token_hash` text not null
- `device_name` text null
- `created_at` timestamp with time zone not null
- `expires_at` timestamp with time zone not null
- `revoked_at` timestamp with time zone null

Indexes and constraints:

- index on `user_id`
- index on `expires_at`

### `categories`

Columns:

- `id` uuid primary key
- `user_id` uuid not null references `users(id)`
- `name` text not null
- `color` text null
- `icon` text null
- `created_at` timestamp with time zone not null
- `updated_at` timestamp with time zone not null

Indexes and constraints:

- unique index on `user_id, name`
- index on `user_id`

### `expenses`

Columns:

- `id` uuid primary key
- `user_id` uuid not null references `users(id)`
- `category_id` uuid null references `categories(id)`
- `title` text not null
- `amount` numeric(12,2) not null
- `expense_date` date not null
- `payment_method` text null
- `note` text null
- `is_recurring` boolean not null default false
- `created_at` timestamp with time zone not null
- `updated_at` timestamp with time zone not null

Indexes and constraints:

- index on `user_id, expense_date`
- index on `user_id, category_id`

### `subscriptions`

Columns:

- `id` uuid primary key
- `user_id` uuid not null references `users(id)`
- `category_id` uuid null references `categories(id)`
- `name` text not null
- `amount` numeric(12,2) not null
- `billing_cycle` text not null
- `renewal_date` date not null
- `is_active` boolean not null default true
- `reminder_days_before` integer not null default 3
- `created_at` timestamp with time zone not null
- `updated_at` timestamp with time zone not null

Indexes and constraints:

- index on `user_id, renewal_date`
- index on `user_id, is_active`

## Phase 2 Tables

### `budgets`

- `id`
- `user_id`
- `category_id` nullable
- `monthly_limit`
- `month`
- `year`

Recommended unique rule:

- unique index on `user_id, category_id, month, year`

### `notifications`

- `id`
- `user_id`
- `type`
- `message`
- `is_read`
- `created_at`

## Recurrence Design Note

Start with simple recurrence enum-like fields in phase 2 unless requirements expand. Do not introduce a separate recurrence rule system until simple schedules are insufficient.

## Data Isolation Rule

Every user-owned table must be queried and mutated with authenticated `user_id` scoping.
