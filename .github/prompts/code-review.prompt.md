# Code Review

Review the following code for Clean Architecture compliance and quality.

## Checklist

### Architecture
- [ ] Dependency direction respected (Domain ← Application ← Infrastructure ← Api)
- [ ] No EF Core or HTTP clients referenced from Application
- [ ] No business logic in endpoints — only `mediator.Send()`
- [ ] Domain entity uses factory method, raises events if needed
- [ ] No `new` keyword for services in handlers

### Commands & Queries
- [ ] Command returns `Result<T>` (not void, not exceptions for expected failures)
- [ ] Validator exists for every command
- [ ] Validator covers all properties and edge cases
- [ ] Handler constructor injects dependencies (no field-level `new`)

### Code Quality
- [ ] `sealed` on all classes that are not intended for inheritance
- [ ] No public setters on domain entities
- [ ] No magic strings — use constants or enums
- [ ] No commented-out code
- [ ] XML summary docs on public APIs

### Tests
- [ ] Unit tests cover happy path and all validation rules
- [ ] Integration test covers the HTTP round-trip
- [ ] No real infrastructure in unit tests (all mocked with NSubstitute)

## Code to review
[PASTE_CODE_HERE]
