---
name: refactor
description: 'Surgical code refactoring to improve maintainability without changing behavior. Covers extracting functions, renaming variables, breaking down god functions, improving type safety, eliminating code smells, and applying design patterns. Less drastic than a rewrite; use for gradual improvements.'
---

# Refactor

## Overview

Improve code structure and readability without changing external behavior. Refactoring is gradual evolution, not revolution. Use this for improving existing code, not rewriting from scratch.

## When to Use

Use this skill when:
- Code is hard to understand or maintain
- Functions/classes are too large
- Code smells need addressing
- Adding features is difficult due to code structure
- User asks "clean up this code", "refactor this", "improve this"

## The Golden Rules

1. **Behavior is preserved** — refactoring doesn't change what the code does, only how
2. **Small steps** — make tiny changes, run tests after each
3. **Version control is your friend** — commit before and after each safe state
4. **Tests are essential** — without tests, you're not refactoring, you're editing
5. **One thing at a time** — don't mix refactoring with feature changes

## When NOT to Refactor

- Code that works and won't change again
- Critical production code without tests (add tests first)
- When you're under a tight deadline
- "Just because" — need a clear purpose

## Common Code Smells & Fixes

### 1. Long Method
Break functions > 30 lines into smaller, named methods using Extract Method pattern.

### 2. Duplicated Code
Extract repeated logic into shared helpers, extension methods, or base classes.

### 3. Large Class (God Object)
Split using Single Responsibility: identify distinct responsibilities and extract to new classes.

### 4. Long Parameter List
Introduce a Parameter Object (`record` in C#) to group related parameters.

### 5. Feature Envy
Move logic to the class that owns the data it operates on.

### 6. Primitive Obsession
Wrap domain concepts in value objects (e.g., `Email`, `Money`, `OrderId`).

### 7. Magic Numbers/Strings
Replace with named constants or enums.

### 8. Nested Conditionals (Arrow Code)
Use guard clauses / early returns to flatten nesting.

### 9. Dead Code
Delete it. Git has history.

### 10. Inappropriate Intimacy
If one class reaches deep into another's internals, move the logic closer to the data.

## Safe Refactoring Process

```
1. PREPARE
   - Ensure tests exist (write them if missing)
   - Commit current state

2. IDENTIFY
   - Find the code smell to address
   - Plan the refactoring

3. REFACTOR (small steps)
   - Make one small change
   - Run tests
   - Commit if tests pass
   - Repeat

4. VERIFY
   - All tests pass
   - Manual testing if needed
   - Performance unchanged or improved

5. CLEAN UP
   - Update comments
   - Final commit
```

## Refactoring Checklist

### Code Quality
- [ ] Functions are small (< 30 lines)
- [ ] Functions do one thing
- [ ] No duplicated code
- [ ] Descriptive names (no abbreviations)
- [ ] No magic numbers/strings
- [ ] Dead code removed

### Structure
- [ ] Related code is cohesive
- [ ] No circular dependencies
- [ ] Dependency direction is correct

### Type Safety (C#)
- [ ] `record` types used for DTOs and value objects
- [ ] Nullable reference types enabled; no `!` suppression without justification
- [ ] No weakly-typed collections (`List<object>`)

### Testing
- [ ] Refactored code has test coverage
- [ ] All tests pass after each step
