---
name: create-implementation-plan
description: 'Create a new implementation plan file for new features, refactoring existing code or upgrading packages, design, architecture or infrastructure.'
---

# Create Implementation Plan

## Primary Directive

Your goal is to create a new implementation plan file for `${input:PlanPurpose}`. Your output must be machine-readable, deterministic, and structured for autonomous execution by AI systems or humans.

## Output File Specifications

- Save implementation plan files in `/plan/` directory
- Use naming convention: `[purpose]-[component]-[version].md`
- Purpose prefixes: `upgrade|refactor|feature|data|infrastructure|process|architecture|design`
- Example: `feature-orders-module-1.md`, `refactor-domain-layer-1.md`

## Plan Structure

Plans must consist of discrete, atomic phases containing executable tasks. Each phase must be independently processable without cross-phase dependencies unless explicitly declared.

Each task must:
- Have a unique identifier (e.g., `TASK-001`)
- Include specific file paths and implementation details
- Be independently executable (or declare dependencies on other tasks)
- Have measurable completion criteria

## Required Template

All implementation plans must use this template exactly:

```markdown
---
goal: [Concise Title Describing the Plan's Goal]
version: 1.0
date_created: YYYY-MM-DD
status: 'Planned'
tags: [feature|upgrade|refactor|architecture|migration|bug]
---

# [Plan Title]

![Status: Planned](https://img.shields.io/badge/status-Planned-blue)

[A short concise introduction to the plan and the goal it is intended to achieve.]

## 1. Requirements & Constraints

[Explicitly list all requirements & constraints that affect the plan.]

- **REQ-001**: Requirement 1
- **SEC-001**: Security Requirement 1
- **CON-001**: Constraint 1
- **GUD-001**: Guideline 1
- **PAT-001**: Pattern to follow 1

## 2. Implementation Steps

### Phase 1: [Phase Name]

- GOAL-001: [Describe the goal of this phase]

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-001 | Description of task 1 | | |
| TASK-002 | Description of task 2 | | |

### Phase 2: [Phase Name]

- GOAL-002: [Describe the goal of this phase]

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-003 | Description of task 3 | | |

## 3. Alternatives

- **ALT-001**: [Alternative approach 1 — why not chosen]
- **ALT-002**: [Alternative approach 2 — why not chosen]

## 4. Dependencies

- **DEP-001**: [Library, framework, or prerequisite task]
- **DEP-002**: [External dependency]

## 5. Files

- **FILE-001**: `path/to/file.cs` — [what changes]
- **FILE-002**: `path/to/file2.cs` — [what changes]

## 6. Testing

- **TEST-001**: [Unit test to add/update]
- **TEST-002**: [Integration test to add/update]

## 7. Risks & Assumptions

- **RISK-001**: [Risk and mitigation]
- **ASSUMPTION-001**: [Assumption being made]

## 8. Related Specifications / Further Reading

- [Link to related ADR or spec]
- [Link to relevant documentation]
```
