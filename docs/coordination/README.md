# Coordination Docs

This folder is the handoff layer between planning and implementation work.

Use it to keep the planner/product-owner agent and the implementer agent aligned
without relying on chat history.

## What Lives Here

- `OPERATING_MODEL.md` defines the planner vs implementer workflow.
- `BOARD.md` shows the current queue and status of active work.
- `FEATURES.md` stores the current feature and story definitions.
- `BUGFIXES.md` stores bug tickets when defects appear.
- `ROADMAP.md` translates the product plan into execution phases.
- `../WORKLOG.md` records short dated notes about work that started, shipped, or
  got blocked.

## Reading Order

### Planner / Product-Owner Agent

1. `docs/shared/product-scope.md`
2. `docs/shared/domain-model.md`
3. `docs/shared/api-contract.md`
4. `docs/coordination/OPERATING_MODEL.md`
5. `docs/coordination/ROADMAP.md`
6. `docs/coordination/BOARD.md`
7. `docs/coordination/FEATURES.md`

### Implementer Agent

1. `docs/coordination/OPERATING_MODEL.md`
2. `docs/coordination/BOARD.md`
3. `docs/WORKLOG.md`
4. The feature or bug entry referenced by the board
5. Shared and area-specific docs for the code being changed

## Ground Rules

- The planner agent creates or refines stories, priorities, and handoff notes.
- The implementer agent picks up work that is already marked `Ready` unless the
  user says otherwise.
- Before starting a new item, the implementer should check older `In Review`
  items and update them if their PRs were already merged.
- Shared contract or auth changes must still update the source-of-truth docs in
  `docs/shared/`.
- Agents open pull requests to `main`; humans merge them.
- New unrelated work should start from `main` on a fresh focused branch.
- After opening a PR, switch back to local `main`, delete the local feature branch, and rely on GitHub to auto-delete the remote branch after merge.
