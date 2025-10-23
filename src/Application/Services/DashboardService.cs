using Microsoft.EntityFrameworkCore;
using WorkManagement.Application.Abstractions;
using WorkManagement.Application.Dto;
using WorkManagement.Domain.Entities;

namespace WorkManagement.Application.Services;

public interface IDashboardService
{
    Task<DashboardSummaryDto> GetDashboardAsync(Guid? currentUserId = null, CancellationToken cancellationToken = default);
}

public class DashboardService : IDashboardService
{
    private readonly IAppDbContext _dbContext;

    public DashboardService(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DashboardSummaryDto> GetDashboardAsync(Guid? currentUserId = null, CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;
        var weekStart = now.AddDays(-(int)now.DayOfWeek);

        var activeAccounts = await _dbContext.Accounts.AsNoTracking().CountAsync(cancellationToken);
        var activeProjects = await _dbContext.Projects.AsNoTracking().CountAsync(cancellationToken);
        var openWorkItems = await _dbContext.WorkItems.AsNoTracking().CountAsync(w => w.Status != WorkItemStatus.Completed && w.Status != WorkItemStatus.Archived, cancellationToken);

        var weekLogs = await _dbContext.TimeLogs.AsNoTracking()
            .Where(t => t.End.HasValue && t.Start >= weekStart)
            .ToListAsync(cancellationToken);
        var hoursLogged = weekLogs.Sum(t => (t.End!.Value - t.Start).TotalHours);

        var kpis = new List<KpiCardDto>
        {
            new("Active Projects", activeProjects.ToString(), "+3% vs last week", "accent-projects"),
            new("Open Work", openWorkItems.ToString(), "-5% vs last week", "accent-work"),
            new("Hours Logged", hoursLogged.ToString("0.0"), "+8% vs last week", "accent-hours"),
        };

        var myWorkItems = currentUserId.HasValue
            ? await _dbContext.WorkItems.AsNoTracking()
                .Include(w => w.Project)
                .Include(w => w.TimeLogs)
                .Where(w => w.AssigneeId == currentUserId && w.Status != WorkItemStatus.Completed && w.Status != WorkItemStatus.Archived)
                .OrderBy(w => w.DueDate ?? w.CreatedAt)
                .Take(10)
                .Select(w => new MyWorkItemDto(
                    w.Id,
                    w.Title,
                    w.Project != null ? w.Project.Name : string.Empty,
                    w.Status.ToString(),
                    w.DueDate,
                    w.TimeLogs.Sum(t => ((t.End ?? t.Start) - t.Start).TotalHours)))
                .ToListAsync(cancellationToken)
            : Array.Empty<MyWorkItemDto>();

        var recentActivity = await _dbContext.Comments.AsNoTracking()
            .Include(c => c.Author)
            .Include(c => c.WorkItem)
            .OrderByDescending(c => c.CreatedAt)
            .Take(10)
            .Select(c => new ActivityFeedItemDto(c.Id, $"{c.Author!.DisplayName} commented on {c.WorkItem!.Title}", c.CreatedAt, "comment"))
            .ToListAsync(cancellationToken);

        return new DashboardSummaryDto(
            activeAccounts,
            activeProjects,
            openWorkItems,
            hoursLogged,
            kpis,
            myWorkItems,
            recentActivity);
    }
}
