# Contributing to MyFi

This project uses a lightweight Git workflow designed for two collaborators working in the same repository.

## Core Rules

- Do not push directly to `main` for normal work.
- Create a branch for each task, fix, or document update.
- Keep branches small and short-lived.
- Open a pull request before merging into `main`.
- Keep `main` stable, reviewable, and usable at all times.

## Branching Strategy

- `main` is the shared stable branch.
- Each change should start from the latest `main`.
- Use one branch per focused piece of work.
- If a change touches both frontend and backend for the same feature, keep it in one branch.

## Branch Naming

Use clear branch names that reflect the type of work:

- `feat/frontend-login-page`
- `feat/backend-auth-endpoints`
- `fix/dashboard-total-calculation`
- `docs/api-contract-update`
- `chore/repo-setup`

## Day-To-Day Flow

1. Check whether older `In Review` work has been merged, and update its status if needed.
2. Update local `main`.
3. Create a new branch from `main` unless the work clearly belongs to the current branch.
4. Make focused changes.
5. Commit in small logical steps.
6. Push the branch to GitHub.
7. Open a pull request.
8. Switch back to local `main` and delete the local feature branch copy.
9. Merge after review and conflict resolution.
10. Let GitHub auto-delete the remote branch after merge.

Typical commands:

```bash
git checkout main
git pull origin main
git checkout -b feat/some-task

# work, then commit
git add .
git commit -m "Add some task"

git push -u origin feat/some-task

# open PR, then clean up local branch
git checkout main
git branch -d feat/some-task
```

## Pull Request Rules

- Keep pull requests small enough to review quickly.
- Do not mix unrelated work in one PR.
- Write a clear title that explains the change.
- Mention any API contract or shared doc changes in the PR description.
- Before starting the next unrelated feature, check older PRs that were in review and update their tracked status if they were merged.
- Start unrelated follow-up work from a fresh branch off `main`, not from an older feature branch.
- After opening the PR, switch back to `main` locally and remove the local feature branch.
- Keep the repository setting enabled to automatically delete head branches after merge.
- Merge frequently instead of letting branches drift for too long.

## Merge Conflict Rules

- Pull from `main` regularly if your branch lives longer than a day or two.
- Resolve conflicts on your branch before merging.
- If both people need the same shared file, coordinate early.
- Avoid large rewrites of the same file at the same time.

## Shared Ownership Rules

- `frontend/` is primarily frontend-owned.
- `backend/` is primarily backend-owned.
- `docs/shared/` is shared and should be updated carefully.
- `docs/shared/api-contract.md` is the source of truth for frontend/backend integration.

If an endpoint, DTO, or auth flow changes, update the shared docs in the same branch.

## Commit Guidelines

- Prefer small, logical commits.
- Use clear messages in imperative form.
- Examples:
  - `Add repository agent instructions`
  - `Add README and initial app directories`
  - `Update API contract for subscription status`

## When Direct Pushes Are Acceptable

Direct pushes to `main` should be avoided even with two collaborators.
The only reasonable exception is an urgent tiny fix, but the default should still be branch + PR for consistency.

## Review Expectations

- Reviews can stay lightweight.
- For normal changes, a quick pass from the other person is enough.
- For shared contract, auth, or architecture changes, both people should understand the impact before merge.

## Practical Team Agreement

- Keep `main` clean.
- Use feature branches for all normal work.
- Open PRs early for visibility.
- Merge small changes often.
- Communicate before editing shared docs or integration boundaries.
