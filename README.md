# Accounts · Projects · Work Management Dashboard

A cross-platform dashboard application built with .NET 9, Blazor Hybrid (MAUI), and DevExpress v25.1 for managing accounts, projects, and work items. The solution targets Windows desktop and Android mobile form factors with optional iOS/macOS support. It delivers a Spotify-inspired user experience with rich data visualizations, offline capabilities, and built-in time tracking.

## Project Vision

* Provide a unified workspace to navigate the hierarchy `Account → Project → Work Item → Comment`.
* Offer immersive, dark-mode-first dashboards, drilldowns, and DevExpress-powered data grids, schedulers, and charts.
* Enable collaboration via comments, notifications, and timers while maintaining offline access with conflict-aware sync.

## High-Level Architecture

| Layer | Responsibilities |
| --- | --- |
| **Client (MAUI Blazor Hybrid)** | DevExpress UI components, navigation shell, local SQLite cache, offline queueing, background sync, device integrations (notifications, file pickers). |
| **Server (ASP.NET Core minimal APIs)** | Identity & JWT auth, role-based authorization, EF Core 9 data access, SQL Server persistence, reporting endpoints, file access via signed URLs. |
| **Sync & Integration** | Delta-based push/pull synchronization, merge policies, activity stream broadcasting, telemetry and logging via Serilog/OpenTelemetry. |

Additional architectural details are captured in [`docs/architecture-overview.md`](docs/architecture-overview.md).

## Core Domains

* **Accounts** – customers and suppliers with contacts, tags, and related projects.
* **Projects** – lifecycle metadata, templates, scheduling, and attachments.
* **Work Items** – hierarchical tasks/tickets with comments, timers, and Kanban status.
* **Time Tracking & Reporting** – timers, time logs, SLA metrics, and export pipelines.

Entity definitions, relationships, and data constraints are described in [`docs/data-model.md`](docs/data-model.md).

## UX & Component Strategy

The user interface leans on DevExpress components for navigation, data visualization, and reporting. Detailed mappings between experience areas and component choices reside in [`docs/devexpress-component-map.md`](docs/devexpress-component-map.md).

A Spotify-style dark theme with rounded cards, accent tagging, and animated drilldowns anchors the design. More UX notes and state management considerations are highlighted in [`docs/architecture-overview.md`](docs/architecture-overview.md#user-experience--state-management).

## Project Structure

The proposed solution combines a MAUI Blazor Hybrid client, ASP.NET Core API, shared domain libraries, and infrastructure packages. Suggested folders, projects, and namespaces are documented in [`docs/folder-structure.md`](docs/folder-structure.md).

## Delivery Roadmap

Milestones covering foundation, core domain builds, collaboration features, reporting, and polish are outlined in [`docs/milestones.md`](docs/milestones.md).

## Getting Started

1. Install **.NET SDK 9.0** (preview) and DevExpress **v25.1** workloads locally. For MAUI development, ensure the `maui` workload plus platform-specific toolchains are available (`dotnet workload install maui`).
2. Restore dependencies for the entire solution:

   ```bash
   dotnet restore WorkManagement.sln
   ```

3. Run the minimal API back end with the in-memory seed data:

   ```bash
   dotnet run --project src/Server/WorkManagement.Server.csproj
   ```

   The server exposes Swagger UI at `https://localhost:7162/swagger` and seeds demo content for dashboard, accounts, projects, and work items.

4. (Optional) Launch the MAUI hybrid client once the API is running. For Windows:

   ```bash
   dotnet build src/Client/WorkManagement.Client.csproj -f net9.0-windows10.0.19041.0
   dotnet run --project src/Client/WorkManagement.Client.csproj -f net9.0-windows10.0.19041.0
   ```

   Android builds can be produced with `dotnet build -f net9.0-android`. The MAUI shell now hosts a Blazor WebView that renders the DevExpress dashboard experience defined in `src/Client/Pages`, with shared styles in `src/Client/wwwroot/css`.

5. Update the DevExpress NuGet feed credentials if required by your license prior to restoring packages.

Source, infrastructure, and UI layers now live in the `src/` directory. The solution file `WorkManagement.sln` includes all projects for convenient IDE or CLI workflows.

## Contributing

1. Create a feature branch from `work`.
2. Align with the architectural docs before introducing new modules.
3. Include automated tests (xUnit, bUnit, Playwright) where applicable.
4. Update documentation when domain models or UX flows change.

## License

Project licensing to be defined.
