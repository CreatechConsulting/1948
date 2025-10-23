# DevExpress Component Map

This guide maps screens and interaction patterns to DevExpress v25.1 components within the MAUI Blazor Hybrid client. It serves as a reference when implementing UI modules and ensures consistency across desktop and mobile experiences.

## Global Shell

| Feature | Component | Notes |
| --- | --- | --- |
| Hierarchical navigation | `DxTreeView` | Renders Accounts → Projects → Work Items tree with lazy loading. |
| Slide-out navigation | `DxDrawer` | Hosts shortcuts, filters, profile actions; adapts to mobile via responsive breakpoints. |
| Breadcrumb trail | `DxBreadcrumbs` | Reflects current drilldown path and supports quick jumps. |
| Theme toggle & user menu | `DxDropDown` + custom toolbar | Integrates avatar, status, and dark-mode controls. |
| Global search | `DxAutocomplete` | Provides fuzzy search suggestions spanning accounts, projects, work items. |

## Dashboard (Home)

| Area | Component | Details |
| --- | --- | --- |
| KPI cards | `DxCard` + `DxGauge` | Displays Active Projects, Overdue Items, Hours Logged with soft glow styling. |
| My Work list | `DxGrid` | Virtualized grid filtered to assigned work items with quick actions. |
| Activity feed | `DxTimeline` | Chronological events (comments, status changes, merges). |
| Time tracker bar | `DxToolbar` + custom timer controls | Floating bottom bar with play/pause/stop, integrates with `DxPopup` for log notes. |

## Accounts Module

| View | Component | Notes |
| --- | --- | --- |
| Account list | `DxGrid` with master-detail | Displays account summary with expand rows for contacts and tags. |
| Account detail | `DxFormLayout` | Editable fields with validation, includes `DxTagBox` for labels. |
| Projects tab | `DxDataGrid` or `DxTreeList` | Shows linked projects with status chips. |
| Files tab | `DxFileManager` | Uses signed URLs for previews/downloads. |

## Projects Module

| View | Component | Notes |
| --- | --- | --- |
| Project list | `DxGrid` with column chooser | Grouping by status, owner, or due date. |
| Board (Kanban) | `DxKanban` | Swimlanes by status/assignee with drag-and-drop and WIP limits. |
| Gantt | `DxGantt` | Timeline for stages and milestones; integrates dependencies and progress. |
| Calendar | `DxScheduler` | Displays project events, deadlines, and milestones. |
| Detail overview | `DxTabs` + `DxFormLayout` | General info, team, notes, attachments. |

## Work Items Module

| View | Component | Notes |
| --- | --- | --- |
| Tree | `DxTreeList` | Nested hierarchy for epics → tasks → sub-tasks. |
| List | `DxGrid` | Inline editing, priority tags via `DxBadge`. |
| Detail pane | `DxSplitLayout` | Left: metadata form; Right: `DxTabs` for comments, history, attachments. |
| Comments | `DxList` + custom template | Real-time updates with mention suggestions (`DxAutocomplete`). |
| Attachments | `DxUpload` + `DxFileManager` | Drag/drop uploads with progress indicators. |
| Filters | `DxComboBox`, `DxTagBox`, `DxDateRangeBox` | Persist saved views. |

## Time Tracking & Reporting

| View | Component | Notes |
| --- | --- | --- |
| Timer dialog | `DxPopup` | Captures notes when stopping timers; includes `DxSpinEdit` for adjustments. |
| Time log grid | `DxGrid` | Group by user/project, export via toolbar. |
| Charts | `DxChart` | Stacked area for velocity, bar charts for SLA breaches, donut for effort distribution. |
| Export viewer | DevExpress Reporting Viewer | Displays PDF/Excel reports with parameter inputs. |

## Notifications & Collaboration

| Feature | Component | Notes |
| --- | --- | --- |
| In-app notifications | `DxDropDownButton` + `DxList` | Bell icon with unread badges. |
| Watch list management | `DxTagBox` | Manage watchers for projects/work items. |
| Mentions | `DxAutocomplete` | Suggests users in comment composer. |
| Activity overlay | `DxDrawer` | Slide-in panel showing sync status, pending changes. |

## Settings & Administration

| View | Component | Notes |
| --- | --- | --- |
| User & role grid | `DxGrid` | Bulk actions, inline status toggles. |
| Role detail | `DxTreeList` | Displays permissions matrix. |
| Theme settings | `DxColorEdit`, `DxSlider` | Adjust accent colors, glow intensity. |
| Integrations | `DxFormLayout` | Configure external storage, notifications, webhooks. |

## Responsive & Platform Notes

* DevExpress adaptive layout features (Breakpoints, `DxToolbar` responsive groups) tailor UI to tablet/phone layouts.
* `DxPopup` and `DxDrawer` leverage MAUI platform APIs to present native-feeling modals and flyouts.
* Keyboard shortcuts on desktop use `DxCommand` / command manager, while mobile surfaces quick actions via floating buttons.

