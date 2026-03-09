---
name: architecture-blueprint-generator
description: 'Comprehensive project architecture blueprint generator that analyzes codebases to create detailed architectural documentation. Automatically detects technology stacks and architectural patterns, generates visual diagrams, documents implementation patterns, and provides extensible blueprints for maintaining architectural consistency.'
---

# Comprehensive Project Architecture Blueprint

Create a `docs/Project_Architecture_Blueprint.md` document that thoroughly analyzes the architectural patterns in the codebase to serve as a definitive reference for maintaining architectural consistency.

## Analysis Approach

### 1. Architecture Detection

Analyze the project structure to identify:
- Technology stacks and frameworks (examine `*.csproj`, `package.json`, imports)
- Architectural pattern (Clean Architecture, Layered, Microservices, CQRS, etc.)
- Folder organization and namespacing
- Dependency flow and component boundaries

### 2. Architectural Overview

- Provide a clear, concise explanation of the overall architectural approach
- Document guiding principles evident in the architectural choices
- Identify architectural boundaries and how they're enforced
- Note any hybrid architectural patterns

### 3. Architecture Visualization

Create a Mermaid diagram showing:
- High-level architectural overview (layers/subsystems)
- Component interaction and dependency directions
- Data flow through the system

### 4. Core Architectural Components

For each component:
- **Purpose**: Primary function and business domain it serves
- **Internal Structure**: Key abstractions and their implementations
- **Interaction Patterns**: How it communicates with other components
- **Extension Points**: How it can be extended without modification

### 5. Layer Structure & Dependencies

- Map the layer structure as implemented
- Document dependency rules between layers (what depends on what)
- Identify abstraction mechanisms that enable layer separation
- Note any violations or exceptions

### 6. Data Architecture

- Domain model structure and organization
- Entity relationships and aggregation patterns
- Data access patterns (repositories, specifications, etc.)
- Data validation approaches

### 7. Cross-Cutting Concerns

Document implementation patterns for:
- **Authentication & Authorization**: Security model, permission enforcement
- **Error Handling**: Exception patterns, Result<T> usage, error responses
- **Logging & Monitoring**: Instrumentation, observability, structured logging
- **Validation**: Input validation, business rule validation
- **Configuration**: Settings management, environment-specific config, secrets

### 8. Testing Architecture

- Testing strategies aligned with the architecture
- Test boundary patterns (unit, integration, E2E)
- Test doubles and mocking approaches
- Test data strategies

### 9. Extension Guide for New Development

Provide a practical guide for adding new features:

```markdown
## Adding a New Feature: Step-by-Step

1. **Domain** (`src/Domain/{Feature}/`):
   - Create the aggregate root entity
   - Define domain events
   - Add value objects

2. **Application** (`src/Application/Features/{Feature}/`):
   - Create Command(s) with validator and handler in one file
   - Create Query(ies) with handler
   - Define DTOs

3. **Infrastructure** (`src/Infrastructure/Persistence/`):
   - Add EF Core entity configuration
   - Register in DbContext

4. **API** (`src/Api/Endpoints/`):
   - Create endpoint class mapping to commands/queries
   - Register in Program.cs

5. **Tests** (`tests/`):
   - Unit tests for validators and handlers
   - Integration tests for endpoints
```

### 10. Common Pitfalls

Document architecture violations to avoid based on the actual codebase:
- Dependency direction violations
- Leaking domain logic into infrastructure
- Bypassing MediatR pipeline
- Putting business logic in endpoints

---

*Include information about when this blueprint was generated and recommendations for keeping it updated as the architecture evolves.*
