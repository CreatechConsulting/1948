# Proposed Solution & Folder Structure

The repository will be organized as a multi-project .NET workspace separating client, server, shared libraries, and infrastructure assets. This structure promotes clean layering, reuse, and maintainability across MAUI, Blazor, and API surfaces.

```
/ (repo root)
├── src/
│   ├── Client/
│   │   ├── WorkDashboard.App/                 # .NET MAUI Blazor Hybrid app
│   │   │   ├── App.xaml, MauiProgram.cs
│   │   │   ├── Resources/                     # Fonts, images, raw assets
│   │   │   ├── Platforms/                     # Android, Windows specific setup
│   │   │   └── wwwroot/                       # Static web assets, CSS, JS
│   │   └── WorkDashboard.App.Tests/           # bUnit & Playwright tests
│   │
│   ├── Server/
│   │   ├── WorkDashboard.Api/                 # ASP.NET Core minimal APIs
│   │   │   ├── Program.cs, extensions/
│   │   │   ├── Modules/                       # Feature folders per aggregate
│   │   │   ├── Infrastructure/                # EF Core, Identity, blob providers
│   │   │   └── Reporting/                     # DevExpress report definitions
│   │   ├── WorkDashboard.Api.Tests/           # xUnit integration tests
│   │   └── WorkDashboard.Api.ContractTests/   # Sync payload verification
│   │
│   ├── Shared/
│   │   ├── WorkDashboard.Domain/              # Entities, value objects, domain events
│   │   ├── WorkDashboard.Application/         # CQRS handlers, validators, services
│   │   ├── WorkDashboard.Sync/                # Shared contracts & DTOs for sync engine
│   │   └── WorkDashboard.Infrastructure/      # Reusable infrastructure (logging, caching)
│   │
│   └── Tooling/
│       ├── WorkDashboard.Build/               # Cake / Nuke build scripts
│       └── WorkDashboard.DevExpress/          # Custom DevExpress theme assets
│
├── tests/
│   ├── WorkDashboard.LoadTests/               # K6 or NBomber load scenarios
│   └── WorkDashboard.UITests/                 # Cross-platform Playwright/MSTest UI harness
│
├── docs/                                      # Architecture & planning docs
├── assets/                                    # UX mocks, color palettes, icons
├── build/                                     # CI/CD pipeline definitions, templates
├── tools/                                     # Scripts, formatters, git hooks
├── Directory.Build.props                      # Shared MSBuild configuration
├── WorkDashboard.sln                          # Root solution file
└── README.md
```

## Naming Conventions

* Projects adopt the `WorkDashboard.*` prefix for clarity within solutions and NuGet feeds.
* Namespaces mirror folder paths (e.g., `WorkDashboard.Application.WorkItems`).
* Feature folders under `Modules/` align with aggregates (Accounts, Projects, WorkItems, Reports).

## Dependency Guidelines

* `WorkDashboard.Domain` contains pure domain logic and references no other project.
* `WorkDashboard.Application` depends on `Domain` and introduces abstractions for persistence and services.
* `WorkDashboard.Infrastructure` implements interfaces from `Application` and is consumed by both API and client (where applicable).
* `WorkDashboard.Api` references `Application`, `Infrastructure`, `Sync`, and `Domain`.
* `WorkDashboard.App` references `Application`, `Sync`, and uses dependency injection to access offline repositories.

## Configuration Management

* Shared build settings in `Directory.Build.props` enforce analyzer rules, nullable reference types, and C# language version.
* `appsettings.*.json` files reside in `WorkDashboard.Api` with environment-specific overrides.
* `WorkDashboard.App` stores environment configs under `Resources/Raw/` and leverages `IOptions` for typed settings.

## Workload & Tooling

* MAUI workloads are restored via `dotnet workload restore` executed from repo root or `src/Client/WorkDashboard.App`.
* DevExpress packages are consumed from a private NuGet feed defined in `NuGet.Config` under `/`.
* Custom themes and SVG assets live in `src/Tooling/WorkDashboard.DevExpress` and are referenced by both app and documentation.

Adopting this structure keeps the solution modular, testable, and ready for scaling into microservices or additional clients if required.

