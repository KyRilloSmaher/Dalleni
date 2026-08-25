# Dalleni Unit Tests

This project is organized by API module so every endpoint can be tested from the outer controller down to application handlers and infrastructure repositories.

## Folder Pattern

Each module should follow this structure:

```text
Modules/
  ModuleName/
    Controllers/
    Commands/
    Queries/
    Repositories/
```

Use `Categories` as the reference module when adding new endpoint coverage.

## Test Types

- `Controllers`: verify route actions call the expected MediatR request and return the expected `IActionResult`.
- `Commands`: verify command handlers perform writes, call repositories, save changes, and return the correct response.
- `Queries`: verify query handlers read from repositories, map DTOs, and return the expected response.
- `Repositories`: verify repository behavior with EF Core InMemory databases.

## Shared Helpers

- `Shared/Builders`: reusable domain and DTO factories.
- `Shared/Infrastructure`: EF Core test database helpers.
- `Shared/Responses`: common response factory helpers.

