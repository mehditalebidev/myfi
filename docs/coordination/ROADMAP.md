# Coordination Roadmap

This roadmap turns the product plan into execution stages that the planner and
implementer agents can manage in markdown.

## Stage 0 - Repository Foundation

Status: in progress

- `SETUP-001` add root README and initial app directories - done
- `SETUP-002` add workspace entrypoints and contributing guide - done
- `SETUP-003` initialize frontend and backend project scaffolds - in progress (backend scaffold is checked in; frontend scaffold still pending)

## Stage 1 - Auth Foundation

Status: in progress

- `AUTH-001` backend auth foundation - in progress
- `AUTH-002` frontend auth shell - not started

Goal: reach the point where the app can establish an authenticated local session
using backend-issued tokens.

## Stage 2 - Core Data Flows

Status: not started

- `CAT-001` category CRUD
- `EXP-001` expense CRUD
- `SUB-001` subscription CRUD

Goal: support the main day-to-day finance management flows inside the MVP.

## Stage 3 - Dashboard And Cohesion

Status: not started

- `DASH-001` dashboard summary
- `QUAL-001` MVP hardening pass

Goal: make the product feel coherent, useful, and stable enough for portfolio use.

## Stage 4 - Deferred Expansion

Status: deferred

- `BUD-001` budgets
- `REP-001` reporting improvements
- `REM-001` reminders
- `CSV-001` CSV import/export

## Planner Notes

- Keep only the near-term next slice in `Ready` if sequence matters heavily.
- Split large vertical slices into smaller stories when implementation starts to
  reveal too much hidden work.
- Prefer end-to-end slices over isolated technical tasks when the scaffold exists.
