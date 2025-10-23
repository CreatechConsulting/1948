# Architecture Overview

This document expands on the end-to-end architecture for the Accounts · Projects · Work Management Dashboard. It frames the system as a set of layered components that collaborate to deliver offline-capable experiences across Windows (WinUI 3) and Android (MAUI) targets.

## Solution Composition

```
+--------------------------------------------------------------+
|                   Cross-Platform Client (MAUI)               |
|  - Blazor Hybrid UI (DevExpress v25.1)                       |
|  - Navigation Shell (Tree, Drawer, Breadcrumbs)              |
|  - Local SQLite Cache & Preferences                          |
|  - Background Sync & Telemetry (Essentials, HttpClient)      |
+--------------------------------------------------------------+
                ↑                         ↓
+--------------------------------------------------------------+
|               Shared Application Layer (Core)                |
|  - Domain Models & DTOs                                      |
|  - Validation & Mapping (FluentValidation, AutoMapper)       |
|  - Command/Query Handlers (Mediator pattern)                 |
|  - Sync Contracts & Change Tracking                          |
+--------------------------------------------------------------+
                ↑                         ↓
+--------------------------------------------------------------+
|                      Server Platform (API)                   |
|  - ASP.NET Core 9 Minimal APIs                               |
|  - EF Core 9 / SQL Server                                    |
|  - Identity + JWT + Refresh Tokens                           |
|  - File Storage (Azure Blob / S3 / On-Prem via providers)     |
|  - DevExpress Reporting Services                             |
+--------------------------------------------------------------+
```

### Client Responsibilities

* Render dark-mode-first UX using DevExpress MAUI Blazor components (`DxTreeView`, `DxGrid`, `DxScheduler`, etc.).
* Provide hierarchical navigation with smooth transitions and breadcrumb awareness.
* Persist the working set locally via SQLite and a lightweight repository abstraction to support offline workflows.
* Host the timer subsystem and notifications using platform services (Android foreground service, Windows tray icon).
* Collect telemetry (Serilog sinks, Application Insights/OpenTelemetry exporters) and funnel logs to the server when connected.

### Server Responsibilities

* Expose secured REST endpoints (JWT bearer + refresh tokens) for accounts, projects, work items, comments, time logs, and attachments.
* Enforce role-based access control aligned with Admin, Project Manager, Contributor, and Viewer roles.
* Implement sync APIs that issue delta feeds and accept queued writes with conflict detection using vector clocks / change stamps.
* Generate reports (time, SLA, velocity) with DevExpress Reporting and stream them as PDF/Excel exports.
* Manage file storage indirection via signed URL issuance and virus scanning hooks.

### Shared Application Layer

* Centralizes domain entities, value objects, and business rules that must stay consistent across client and server.
* Defines mediation contracts (`ICommand`, `IQuery`) to decouple UI from business logic.
* Provides change-tracking helpers used by both offline cache and server-side synchronization.

## Deployment Targets

| Environment | Client Distribution | API Hosting |
| --- | --- | --- |
| Development | Local MAUI app, hot reload, SQLite dev DB | Docker-compose with SQL Server 2022 + ASP.NET Core | 
| Staging | MSIX (WinGet) & Android Internal Track | Azure App Service / Container Apps, managed SQL Database |
| Production | MSIX/Store & Play Store | Azure Kubernetes Service or equivalent with horizontal scaling |

## Security Considerations

* **Authentication**: ASP.NET Core Identity with passwordless options (FIDO2, magic links) layered over JWT/refresh tokens.
* **Authorization**: Policy-based RBAC with row-level filters (EF Core HasQueryFilter) to scope data per account/project membership.
* **Data Protection**: AES encryption for locally cached sensitive fields, TLS 1.2+ enforced end-to-end, optional customer-managed keys for blobs.
* **Auditability**: Append-only audit log stored server-side (SQL table + Azure Table Storage mirror) and surfaced in activity feeds.
* **Rate Limiting**: ASP.NET Core rate limiter to mitigate abuse on public endpoints.

## Offline Sync Pipeline

1. **Change Tracking** – The client records mutations into an append-only local log with entity keys, timestamps, and payload hashes.
2. **Queue Dispatch** – A background service attempts to push queued changes. Items remain `Pending` until server acknowledgement.
3. **Conflict Detection** – Server compares incoming change stamps with its latest known version and either merges, flags conflicts, or rejects.
4. **Merge Resolution** – Non-destructive merges append alternate versions; users receive prompts to resolve using the activity stream.
5. **Delta Pulls** – The client periodically requests deltas since the last sync token to refresh cached aggregates.

Additional details on sync data contracts and retry strategies will be defined alongside implementation tickets.

## Observability & Diagnostics

* **Logging**: Serilog sinks for console, Seq, Application Insights. Client logs are batched offline and uploaded on reconnection.
* **Metrics**: OpenTelemetry instrumentation covering API latency, sync throughput, timer usage, and offline duration.
* **Tracing**: Distributed tracing IDs propagate between client and server to correlate sync batches with backend processing.

## User Experience & State Management

* Dark palette inspired by Spotify with neon-accent highlights, soft shadows, and rounded surfaces.
* Navigation shell combines tree-based exploration with breadcrumbs and contextual drawers for quick actions.
* `Fluxor` or a custom store pattern manages view state, filters, and selections to ensure predictable updates across pages.
* Virtualized lists and skeleton loading handle large datasets while maintaining <200 ms perceived response.
* Animated drilldowns use MAUI `VisualStateManager` and DevExpress transitions to fade/slide between hierarchy levels.

## Testing Strategy

| Layer | Tests |
| --- | --- |
| Shared & Server | xUnit unit tests, integration tests using WebApplicationFactory, EF Core in-memory/SQL containers |
| Client | bUnit component tests, MAUI/Blazor Playwright UI tests, timer logic unit tests |
| Sync | Contract tests validating delta payloads and conflict resolution scenarios |

CI/CD pipelines (GitHub Actions or Azure Pipelines) will execute test suites, run linters (`dotnet format`, `stylecop`), and package distributables (MSIX, APK/AAB). Release pipelines promote artifacts to staging/production after automated quality gates.

