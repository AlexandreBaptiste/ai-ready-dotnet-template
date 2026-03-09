---
name: conventional-commit
description: 'Prompt and workflow for generating conventional commit messages using a structured format. Guides users to create standardized, descriptive commit messages in line with the Conventional Commits specification.'
---

### Instructions

Generate a conventional commit message for my staged changes.

### Workflow

**Follow these steps:**

1. Run `git status` to review changed files.
2. Run `git diff --cached` to inspect staged changes (or `git diff` if nothing staged yet).
3. Stage your changes with `git add <file>` if needed.
4. Construct your commit message using the structure below.
5. Run the `git commit` command in the terminal automatically — no confirmation needed.

### Commit Message Structure

```
type(scope): description

[optional body]

[optional footer]
```

### Types

| Type | When to Use |
|------|-------------|
| `feat` | A new feature |
| `fix` | A bug fix |
| `docs` | Documentation changes only |
| `style` | Formatting, whitespace (no logic change) |
| `refactor` | Code change that neither fixes a bug nor adds a feature |
| `perf` | Performance improvement |
| `test` | Adding or correcting tests |
| `build` | Build system, dependency changes |
| `ci` | CI/CD configuration changes |
| `chore` | Maintenance tasks (cleanup, updating configs) |
| `revert` | Reverts a previous commit |

### Rules

- **type**: Required. Must be one of the types above.
- **scope**: Optional but recommended. The module/area affected (e.g., `orders`, `auth`, `api`).
- **description**: Required. Imperative mood ("add" not "added"), lowercase, no trailing period.
- **body**: Optional. Use for additional context about the *why*, not the *what*.
- **footer**: Use for `BREAKING CHANGE:` or issue references (`Closes #123`).

### Breaking Changes

Append `!` after the type/scope for breaking changes:
```
feat(api)!: change order endpoint response structure

BREAKING CHANGE: OrderDto now returns `customerId` instead of `userId`
```

### Examples

```
feat(orders): add create order endpoint
fix(auth): resolve JWT expiry validation issue
refactor(domain): extract order total calculation to value object
test(orders): add integration tests for create order endpoint
docs: update README with local development setup
chore: upgrade EF Core to 10.0.3
feat(payments)!: replace Stripe v2 with Stripe v3 SDK

BREAKING CHANGE: PaymentResult model fields renamed
```

### Final Step

After generating the message, execute:
```bash
git commit -m "type(scope): description"
```

Include `--message` body and footer if substantial. Replace the template with your constructed message.
