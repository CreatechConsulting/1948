# Delivery Milestones & Workstreams

The roadmap is structured into five milestones, each building on the previous capabilities while keeping the application releasable at the end of every iteration. Timelines assume a two-week sprint cadence with cross-functional collaboration between engineering, UX, and QA.

## Milestone 1 – Foundation (Weeks 1–3)

**Goals**

* Establish repository structure, solution scaffolding, and CI pipeline skeleton.
* Implement authentication shell (Identity + JWT) and secure MAUI shell login flow.
* Apply dark theme baseline, Spotify-inspired styling, and navigation chrome.
* Set up DevExpress packages, licensing, and theming infrastructure.

**Deliverables**

* `WorkDashboard.sln`, initial projects, and dependency injection wiring.
* Login screen with JWT exchange against stubbed API endpoint.
* Shared layout with `DxDrawer`, `DxTreeView`, `DxBreadcrumbs` placeholders.
* GitHub Actions workflow executing `dotnet build` + linting checks.

## Milestone 2 – Core Entities (Weeks 4–7)

**Goals**

* Build CRUD APIs and UI flows for Accounts, Projects, and Work Items.
* Implement EF Core migrations, repositories, and SQLite cache synchronization.
* Deliver key grids, forms, and Kanban view prototypes.

**Deliverables**

* Accounts module with list/detail forms and tagging support.
* Projects module with list, board, and basic Gantt timeline.
* Work items tree/list with CRUD dialogs and inline editing.
* Initial offline cache + delta sync prototype for Accounts/Projects.

## Milestone 3 – Collaboration & Time (Weeks 8–11)

**Goals**

* Introduce comment threads, @mentions, watch lists, and activity feed.
* Embed timer controls, time log capture, and My Work dashboard section.
* Harden sync conflict resolution UI/UX.

**Deliverables**

* Comment composer with mention suggestions and attachments.
* Timer service with background execution, time log grid, and export to CSV.
* Activity feed timeline and notifications drawer.
* Expanded sync coverage for work items, comments, and timers.

## Milestone 4 – Reporting & Offline Polish (Weeks 12–14)

**Goals**

* Build reporting suite (time, SLA, velocity) with DevExpress Reporting Viewer.
* Finalize offline capabilities: retry policies, conflict resolution workflows, backlog replay.
* Implement saved searches, filters, and global search indexing.

**Deliverables**

* Report endpoints and viewer integration for PDF/Excel exports.
* Sync dashboard highlighting pending/out-of-date items.
* Saved view management and fuzzy search service.
* Load/performance test baselines (NBomber/K6 scripts).

## Milestone 5 – Notifications, Packaging & Launch (Weeks 15–16)

**Goals**

* Complete push notifications, desktop toasts, and watch subscriptions.
* Package installers (MSIX/MSI) and mobile artifacts (APK/AAB) with versioning.
* Finalize accessibility audits, localization (en-ZA), and analytics dashboards.

**Deliverables**

* Notification services integrated with platform channels.
* Release pipeline promoting builds to staging/production stores.
* Accessibility fixes, localization resources, and telemetry dashboards.
* Launch checklist including rollback and support processes.

## Ongoing Quality Streams

* **Testing** – Expand coverage with unit, integration, UI, and sync contract tests.
* **DevOps** – Enhance observability, infrastructure-as-code, and disaster recovery plans.
* **UX Feedback** – Collect stakeholder input via regular demos, track improvements in backlog.

