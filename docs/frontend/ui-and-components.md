# UI And Components

## Layout Plan

- top app bar with app name, primary navigation, and user menu
- mobile-friendly navigation pattern from the start
- content area with page title, page actions, and body content

## Shared Components

Recommended shared components:

- `AppShell`
- `PageHeader`
- `StatCard`
- `DataTable` or resource list wrapper
- `EmptyState`
- `ErrorState`
- `LoadingState`
- `ConfirmDialog`
- `FormField`
- `CurrencyDisplay`
- `DateDisplay`

## Feature Components

### Categories

- category list
- category form modal or inline sheet
- category badge with color/icon preview

### Expenses

- expense form
- expense table/list
- filter bar
- expense summary strip

### Subscriptions

- subscription form
- subscription list
- active/inactive status badge
- renewal date chip

### Dashboard

- summary card row
- recent expense list
- upcoming renewal list
- category spending chart placeholder

## Form Guidance

- use React Hook Form with Zod resolver
- validate required fields on client before submit
- keep money and date inputs straightforward
- avoid complex multi-step forms in phase 1

## Responsive Guidance

- forms should stack vertically on narrow screens
- tables should gracefully degrade to card/list layouts on mobile if needed
- action buttons should remain reachable without dense toolbar layouts

## UX Details Worth Preserving

- expense creation should feel quick
- categories should not require excessive metadata
- subscriptions should clearly show next renewal and cost
- dashboard should be readable without chart dependence
