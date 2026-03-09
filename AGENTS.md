# AGENTS.md

## Purpose
This repository is a .NET backend project and should be implemented consistently with the architectural, formatting, documentation, naming, syntax, project structure, database, EF Core, validation, enum, and GUI conventions below.

## Technology stack
- Use .NET 10
- Use EF Core 10
- Use stable C# features appropriate for .NET 10
- Prefer built-in .NET and EF Core functionality over unnecessary third-party packages

## Build quality requirements
- Treat warnings as errors for .NET projects
- New code must compile cleanly with no warnings
- Do not suppress warnings unless there is a clearly documented and justified reason
- Enable and follow standard .NET code analysis
- Enable nullable reference types
- Use best-practice syntax and standard C# coding conventions
- Code must follow normal .NET and C# standards, not personal or ad hoc styles

## Architecture
- Keep database access in a separate Data Access layer
- All direct database access must go through the DA layer
- DA classes must only be called by Service classes
- Controllers and other higher-level layers must not call DA classes directly
- Service classes are the boundary between higher-level application logic and the DA layer
- Service classes must not contain raw SQL or direct database access logic unless there is an explicitly documented exception
- Business logic belongs in Service classes
- Validation belongs in Service classes
- DA classes are responsible for querying, loading, saving, and updating data
- Keep responsibilities clearly separated between Service and DA classes

## Project structure
- Organize the solution into clear layers such as API, Services, DataAccess, and Domain when appropriate
- `Services` must contain service interfaces and service implementations
- `DataAccess` must contain DA interfaces and DA implementations
- Keep each interface and its implementation in the same folder
- Organize `Services` and `DataAccess` by feature or business area
- Use subfolders for grouping when needed
- Avoid unnecessary nesting or overly complex folder structures
- Keep related files close together so they are easy to find and maintain

### Project structure examples
- Good structure:
  - `MyApp.Services/Customer/ICustomerService.cs`
  - `MyApp.Services/Customer/CustomerService.cs`
  - `MyApp.DataAccess/Customer/ICustomerDA.cs`
  - `MyApp.DataAccess/Customer/CustomerDA.cs`

- Good grouped structure:
  - `MyApp.Services/MasterData/Customer/ICustomerService.cs`
  - `MyApp.Services/MasterData/Customer/CustomerService.cs`
  - `MyApp.DataAccess/MasterData/Customer/ICustomerDA.cs`
  - `MyApp.DataAccess/MasterData/Customer/CustomerDA.cs`

- Bad structure:
  - placing interfaces in separate technical folders far away from their implementations
  - creating unnecessary deep nesting for small features
  - mixing controllers, services, and DA classes in the same folder

## Naming conventions
- Data access classes must end with `DA`
  - Example: `CustomerDA`, `TradeImportDA`
- Service classes must end with `Service`
  - Example: `CustomerService`, `TradeImportService`
- Interfaces must begin with `I`
  - Example: `ICustomerService`, `ITradeImportDA`
- Class names, interface names, method names, property names, enum names, and public constants must use PascalCase
- Method parameters, local variables, and private fields must use camelCase
- Do not use underscore prefixes for local variables
  - Bad: `_customer`, `_result`
  - Good: `customer`, `result`
- Property names must follow standard C# naming conventions
- Uppercase property names are not allowed
  - Bad: `CUSTOMERNAME`, `TRADEDATE`
  - Good: `CustomerName`, `TradeDate`
- DbContext `DbSet` properties must follow normal C# property naming conventions
- Do not use all-uppercase or database-style uppercase naming for `DbSet` properties
- `DbSet` property names should be clear, readable, and aligned with the entity naming pattern used in the codebase

## Standard C# syntax rules
- Follow standard Microsoft C# naming conventions
- Use PascalCase for:
  - classes
  - records
  - interfaces
  - enums
  - methods
  - properties
  - public fields
  - public constants
- Use camelCase for:
  - parameters
  - local variables
  - private fields
- Do not use all-uppercase names except where required for external interoperability
- Do not use database-style naming in C# code
- Do not use snake_case in C# types, methods, or properties
- Avoid abbreviations unless they are well-known and already established in the codebase
- Use meaningful, readable names
- Braces must be used consistently and follow normal C# conventions
- Use standard access modifier ordering and standard member ordering
- Keep syntax idiomatic and consistent with standard .NET conventions
- Do not introduce non-standard style choices that conflict with common C# practice

## Database table naming
- Physical database table names must be all lowercase
- Database table names must use the format: `{prefix}_{name}`
- The prefix must be exactly 3 letters
- The prefix should be a short abbreviation of the table name
- The part after the underscore is the table name in lowercase
- The prefix is part of the physical database table name only
- The prefix must not be included in the C# entity or model class name
- Model and entity names should stay clean and business-oriented
- Table naming must be consistent across the schema

### Table examples
- Good table names:
  - `cus_customer`
  - `trd_trade`
  - `inv_invoice`
  - `prt_portfolio`

- Good model names:
  - `Customer`
  - `Trade`
  - `Invoice`
  - `Portfolio`

- Bad table names:
  - `Customer`
  - `CUS_CUSTOMER`
  - `cus_Customer`
  - `customer`
  - `custo_customer`

- Bad model names:
  - `CusCustomer`
  - `TrdTrade`
  - `cus_customer`

## Database column naming
- Physical database column names must be all lowercase
- Column names must use the format: `{prefix}_{field_name}`
- The prefix should match the 3-letter abbreviation used for the table
- The field name part must be lowercase
- Use underscores between words in the field name part when needed
- Column names should not use C# PascalCase naming
- Column naming must be consistent across the table

### Column examples
For table `cus_customer`:
- Good column names:
  - `cus_id`
  - `cus_name`
  - `cus_created_date`
  - `cus_status`

- Bad column names:
  - `id`
  - `customer_id`
  - `cus_Name`
  - `CUS_NAME`

## DbContext conventions
- `DbSet` properties should be named as normal PascalCase properties
- Avoid uppercase names such as `CUSTOMER` or `TRADE`
- Prefer names like:
  - `Customers`
  - `Trades`
  - `CashFlows`
- Keep naming consistent across all contexts

## EF Core mapping conventions
- Prefer EF Core attributes on classes and properties for mappings where practical
- Prefer attributes for physical table names, column names, indexes, keys, foreign keys, string length, precision, and other common mapping metadata
- Keep EF Core mapping close to the entity when that improves readability and maintainability
- Use Fluent API configuration only when attributes are not sufficient, would become unclear, or when central configuration is significantly cleaner
- Physical database table names must follow the 3-letter-prefix convention in lowercase
- Physical database column names must follow the 3-letter-prefix convention in lowercase
- Entity class names must not include the database prefix
- Property names must not include the database prefix
- Keep EF Core configuration clear so the distinction between model or property names and table or column names is obvious

### EF Core attribute examples
- Preferred examples:
  - `[Table("cus_customer")]`
  - `[Column("cus_name")]`
  - `[Index(nameof(CustomerName), Name = "idx_customer_name")]`
- Use attributes on the model when possible instead of scattering simple mappings into separate configuration files

## Validation rules
- Every field in every model must have validation rules defined
- Validation must be implemented in the Service layer
- Services must validate all incoming models before persisting data or performing business operations
- Validation must cover required values, string lengths, allowed ranges, allowed formats, enum values, and cross-field consistency where relevant
- Validation messages must be clear, specific, and user friendly
- Do not rely only on database constraints or UI validation
- UI validation and database constraints may exist, but Service-layer validation is required
- Validation logic must be consistent across create, update, and other write operations

## Enum rules
- Enums must be stored as strings in the database, not integers
- Enums must be exposed as strings in frontend-facing contracts, not integers
- Do not use integer enum values in API payloads, GUI models, or persisted database values unless there is a clearly documented exception
- Enum names must be stable, readable, and safe to expose externally
- Configure EF Core to persist enums as strings
- Keep enum string values consistent across backend, database, and frontend

## Data access rules
- All database interaction must be placed in DA classes
- DA classes must only be called by Service classes
- Controllers, background entry points, endpoints, and other higher-level components must go through Service classes
- DA classes should encapsulate EF Core queries and persistence logic
- DA classes should expose methods with business-meaningful names
- Avoid leaking `IQueryable` outside the DA layer unless there is a specific documented reason
- Prefer async database access where appropriate
- Keep query logic maintainable and readable
- Centralize repeated query patterns when practical

## Service layer rules
- Service classes coordinate business logic and calls to one or more DA classes
- Service classes are responsible for orchestrating access to DA classes
- Service classes should not duplicate query logic that belongs in DA classes
- Validate inputs and enforce business rules in the Service layer
- Every field in every model must be validated in the Service layer
- Keep Service methods focused and cohesive

## ASP.NET Core rules
- ASP.NET Core services must expose OpenAPI
- Configure OpenAPI or Swagger so endpoints can be discovered and inspected
- Controllers must be documented with XML comments
- Public endpoints should be visible through OpenAPI for development and testing purposes

## GUI and UX rules
- Any GUI must be user friendly and clear to understand
- Every screen must be designed for usability, clarity, and efficient workflows
- Every screen must include help text that explains the purpose of the screen
- Every input field must have a user-friendly label
- Every field must have a description or explanatory text so the user understands what it is used for
- Validation messages must be clear and actionable
- Avoid technical or developer-oriented wording in the UI unless the target users explicitly expect it
- Prefer consistent layout, spacing, terminology, and control behavior across screens
- Required fields, optional fields, defaults, and field constraints must be obvious to the user
- Help text should explain both what the screen does and how the most important fields should be used
- Frontend enum values must be shown and handled as readable strings, not integers

## Formatting
- Use CSharpier to format code
- Follow CSharpier best-practice or default formatting settings
- Do not manually format code in ways that conflict with CSharpier
- Generated or modified code should be formatted consistently with CSharpier expectations

## Documentation requirements
- Add XML documentation comments to all interfaces
- Implementations of documented interfaces should inherit documentation where appropriate
- Use `<inheritdoc />` on implementations when appropriate
- Add XML documentation comments to all public properties
- Add XML documentation comments to all private methods
- Add XML documentation comments to controllers
- Prefer complete and useful comments over placeholder comments
- Comments should explain intent and usage, not restate trivial code

## EF Core guidelines
- Configure entities and mappings consistently
- Prefer attributes on the model for simple and common mappings
- Keep EF Core model configuration readable and organized
- Use explicit configuration when it improves clarity
- Configure enums to be stored as strings
- Be careful with tracking, lazy loading behavior, and query performance
- Avoid hidden or surprising database behavior

## Dependency injection
- Register DA classes and Service classes through dependency injection
- Prefer constructor injection
- Do not use service locators or static access patterns unless already required by legacy code

## Code style
- Follow existing repository patterns unless there is a strong reason to improve them
- Prefer clear and maintainable code over clever code
- Keep methods reasonably small and focused
- Use descriptive names
- Avoid unnecessary abstraction

## Recommended .NET quality settings
- Enable warnings as errors
- Enable nullable reference types
- Enable .NET analyzers
- Use the latest recommended analysis level supported by the SDK
- Enforce standard style and naming rules through `.editorconfig` where possible
- Keep build validation strict enough that style and syntax drift is caught early

### Example project settings
```xml
<PropertyGroup>
  <TargetFramework>net10.0</TargetFramework>
  <Nullable>enable</Nullable>
  <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  <EnableNETAnalyzers>true</EnableNETAnalyzers>
  <AnalysisLevel>latest</AnalysisLevel>
</PropertyGroup>