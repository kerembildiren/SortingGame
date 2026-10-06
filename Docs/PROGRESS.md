# Progress & Handoff

Living notes so any session (any machine, any Claude account) can pick up where the last one stopped.
Update this file at the end of every work chunk, before committing. Newest entry on top in the log.

## Current state
- **Active milestone:** M1 (core loop in one grey-box section), not started yet.
- **Last approved milestone:** M0 (2026-10-06).
- **Next concrete step:** M1 content builder (categories, items, shelves, one section as data) + section scene.
- **Blocking / waiting on user:** nothing.

## Open questions for the user
- (none)

## Environment notes
- Work PC: `C:\Users\keremb\Personal\SortingGame`, Unity 6000.6.3f1 via Unity Hub.
- **No git remote yet. Never push from the work PC.** The user will ask to push from their personal account at some point.
- graphify setup on a new machine (once, in project root): `graphify hook install` (git hooks are not versioned) and `graphify claude install` (writes machine-local `.claude/settings.json`; rename it to `.claude/settings.local.json`, which is git-ignored, and revert any duplicate graphify section it adds to CLAUDE.md). `graphify-out/` itself is versioned.

## Session log
### 2026-10-06 — M0 done, approved
- Created URP project, portrait settings, asmdefs, data layer SOs, GameDatabase validation, Wallet + tests (6/6 pass), batchmode tooling `tools/unity.sh`.
- User check: project opens, Play runs, no errors (empty scene as expected).
- Committed M0 locally. Set up graphify (graph in `graphify-out/`, post-commit hook, CLAUDE.md section) and this handoff file.
