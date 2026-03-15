# Domain Model

## Core Principles

- Every finance record belongs to exactly one local application user.
- External authentication identities are linked to the local user, not used as the primary domain identity.
- Expenses and subscriptions are separate concepts even if both involve money leaving the account.
- Recurrence starts simple and can expand later without replacing the base model.

## Phase 1 Entities

### User

Represents the local application account owner.

Key fields:

- `Id`
- `Email`
- `DisplayName`
- `PasswordHash`
- `CreatedAt`
- `UpdatedAt`

Rules:

- One user owns many categories, expenses, subscriptions, and auth identities.
- Email is unique and normalized for login.
- Password hashes are stored only for local email/password auth.
- The user record remains the primary app identity even if additional providers are linked later.

### Category

Represents a user-defined expense classification.

Key fields:

- `Id`
- `UserId`
- `Name`
- `Color`
- `Icon`
- `CreatedAt`
- `UpdatedAt`

Rules:

- Category names should be unique per user.
- Categories are optional on expenses and subscriptions to reduce friction.

### Expense

Represents a one-time financial transaction in phase 1.

Key fields:

- `Id`
- `UserId`
- `CategoryId`
- `Title`
- `Amount`
- `ExpenseDate`
- `PaymentMethod`
- `Note`
- `IsRecurring`
- `CreatedAt`
- `UpdatedAt`

Rules:

- Amount is positive decimal money value.
- `ExpenseDate` is the effective transaction date.
- `IsRecurring` is informational in phase 1 and becomes operational in phase 2.

### Subscription

Represents an active or inactive recurring service.

Key fields:

- `Id`
- `UserId`
- `CategoryId`
- `Name`
- `Amount`
- `BillingCycle`
- `RenewalDate`
- `IsActive`
- `ReminderDaysBefore`
- `CreatedAt`
- `UpdatedAt`

Rules:

- Billing cycle starts as enum-like values: `monthly`, `quarterly`, `yearly`.
- Renewal date drives upcoming renewal display.
- Inactive subscriptions remain stored for history.

## Future Auth Extension

### AuthIdentity

Represents an external login identity such as Google.

Key fields:

- `Id`
- `UserId`
- `Provider`
- `ProviderUserId`
- `ProviderEmail`
- `CreatedAt`
- `LastLoginAt`

Rules:

- `Provider + ProviderUserId` must be unique.
- A user can have multiple auth identities in the future.
- This is not part of the current local-auth bootstrap.

## Phase 2 Entities

### Budget

Represents a monthly limit for overall or category-specific spending.

Key fields:

- `Id`
- `UserId`
- `CategoryId` nullable
- `MonthlyLimit`
- `Month`
- `Year`

### Notification

Represents an in-app reminder or alert.

Key fields:

- `Id`
- `UserId`
- `Type`
- `Message`
- `IsRead`
- `CreatedAt`

## Business Rules To Preserve

- All reads and writes are scoped by authenticated user id.
- Money values should use decimal types, never floating point.
- Soft complexity is preferred over over-generalization; phase 1 should not be modeled like enterprise finance software.
- Local auth currently lives on the `User` model; provider identities can be added later without changing ownership boundaries.
- Subscriptions should not automatically create expenses in phase 1.
- Dashboard summary is a projection over user-owned records, not a separate stored aggregate.
