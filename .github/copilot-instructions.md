# Copilot instructions

**Before any development or code generation, always systematically read all instruction files in `.github/instructions/`.** This is mandatory to ensure all project rules (DDD, Object Calisthenics, etc.) are loaded and applied correctly.

## Language Policy

All instructions and prompts in this repository must be written in English. This applies to:
- All rule and instruction files in `.github/instructions/`
- All prompt files in `.github/prompts/`
- All documentation and code comments intended for contributors

## Development code generation

When working with Csharp code, follow these instructions very carefully.

It is **EXTREMELY important that you follow the instructions in files very carefully.**

### Workflow implementation

**IMPORTANT:** Always follow these steps when implementing new features:

1. Consult any relevant instructions files listed below and start by listing which instruction files have been used to guide the implementation (e.g. `Instructions used: [csharp.instructions.md, playwright-dotnet.instructions.md]`).

2. When working with csharp code, Always run `dotnet test` or `dotnet build` to verify that all tests pass before committing your changes.
   Don't ask to run the tests, just do it. If you are not sure how to run the tests, ask for help.
   You can also use `dotnet watch test` to run the tests automatically when you change the code.
   
3. Fix any compiler warnings and errors before going to the next step.

When you see paths like `/[project]/features/[feature]/` in rules, replace [project] with the name of the project you are working on (e.g. `Ordering`), and `[feature]` with the name of the feature you are working on (e.g. `VerifyOrAddPayment`).

## Rule Usage Traceability

Whenever you use a rule from any instruction file, you must explicitly state in your prompt or code generation output which rule(s) have been used, by listing the relevant instruction file(s) as a clean, bulleted list.
