# Coordination Operating Model

This project uses two complementary agent roles.

## Roles

### Planner / Product-Owner Agent

The planner agent manages scope, sequencing, and handoff quality.

Responsibilities:

- break roadmap work into clear feature or bug entries
- keep `BOARD.md`, `FEATURES.md`, `BUGFIXES.md`, and `ROADMAP.md` current
- move items into `Ready` only when the implementation target is clear
- add acceptance criteria, dependencies, and doc-update notes

Constraints:

- do not implement product code as the planner role
- do not mark work `Done` until the related PR is merged by a human
- do not silently change shared contracts without updating shared docs

### Implementer Agent

The implementer agent executes one approved unit of work at a time.

Responsibilities:

- pick a single `Ready` item from `BOARD.md` unless the user directs another item
- create or continue the focused branch for that item
- update the board status when work starts, when a PR is opened, and when it is blocked
- implement the change, update affected docs, and add a short note to `docs/WORKLOG.md`
- push the branch and open a PR to `main`

Constraints:

- do not start unplanned work that is only in chat and not represented in the coordination docs unless the user explicitly overrides that rule
- do not merge pull requests; humans handle merges in this repository
- keep one story or bug actively in progress by default

## Status Model

- `Draft`: rough idea, not ready for implementation
- `Backlog`: approved idea, not yet prepared for pickup
- `Ready`: clear enough for an implementer to start
- `In Progress`: currently being worked on in a branch
- `In Review`: branch pushed and PR opened, waiting for human review or merge
- `Blocked`: cannot continue because of dependency, ambiguity, or external input
- `Done`: merged to `main`

## Item IDs

Use stable IDs so board updates stay easy to follow.

- Setup and process work: `SETUP-###`
- Auth work: `AUTH-###`
- Categories: `CAT-###`
- Expenses: `EXP-###`
- Subscriptions: `SUB-###`
- Dashboard/reporting: `DASH-###`, `REP-###`
- Budgets/reminders/import-export: `BUD-###`, `REM-###`, `CSV-###`
- Docs-only operational work: `DOCS-###`
- Bugfixes: `BUG-###`

## Required Implementer Flow

1. Read `OPERATING_MODEL.md`, `BOARD.md`, and the selected item entry.
2. Start from the latest `main`.
3. Create a focused branch such as `feat/expense-crud`, `fix/auth-refresh`, or
   `docs/coordination-system`.
4. Move the selected item to `In Progress` and note the branch name if useful.
5. Implement the change and update shared docs if the contract or architecture moved.
6. Add a dated note to `docs/WORKLOG.md`.
7. Push the branch and open a PR to `main`.
8. Switch back to local `main` and delete the local feature branch copy.
9. Move the item to `In Review` with the PR link.
10. After a human merges the PR, let GitHub auto-delete the remote branch and then move the item to `Done`.

## Definition Of Ready

An item can move to `Ready` when it includes:

- a clear problem or goal statement
- scope boundaries
- acceptance criteria
- known dependencies
- doc update expectations if contracts or architecture may change

## Definition Of Done

An item is done only when:

- implementation is complete for the agreed scope
- relevant docs are updated
- tests or checks appropriate to the current scaffold level were run, or the
  item clearly notes why they were not runnable yet
- the branch is pushed and the PR was merged to `main` by a human

## Required Files To Keep Updated

- `docs/coordination/BOARD.md`: current status and queue
- `docs/coordination/FEATURES.md` or `docs/coordination/BUGFIXES.md`: source entry
- `docs/WORKLOG.md`: dated progress notes
- `docs/shared/api-contract.md`: only if endpoint shapes or DTOs changed
- `docs/shared/auth-flow.md`: only if auth behavior changed

## Escalation Rules

- If a task uncovers missing scope, send it back to the planner layer by updating
  the relevant feature entry instead of inventing hidden scope.
- If a task requires a cross-cutting contract change, document that change first
  before or alongside code edits.
- If multiple items depend on the same unfinished foundation, keep the dependent
  items in `Backlog` or `Blocked` until the prerequisite lands.
