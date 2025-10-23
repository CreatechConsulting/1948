using System.Collections.Generic;

namespace WorkManagement.Application.Dto;

public record DashboardSummaryDto(
    int ActiveAccounts,
    int ActiveProjects,
    int OpenWorkItems,
    double HoursLoggedThisWeek,
    IEnumerable<KpiCardDto> Kpis,
    IEnumerable<MyWorkItemDto> MyWork,
    IEnumerable<ActivityFeedItemDto> RecentActivity);

public record KpiCardDto(string Title, string Value, string Trend, string Accent);

public record MyWorkItemDto(Guid WorkItemId, string Title, string ProjectName, string Status, DateTimeOffset? DueDate, double LoggedHours);

public record ActivityFeedItemDto(Guid Id, string Description, DateTimeOffset CreatedAt, string Category);
