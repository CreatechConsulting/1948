using Microsoft.EntityFrameworkCore;
using WorkManagement.Application.Abstractions;
using WorkManagement.Application.Dto;
using WorkManagement.Domain.Entities;

namespace WorkManagement.Application.Services;

public interface IProjectReadService
{
    Task<IReadOnlyList<ProjectSummaryDto>> GetProjectsAsync(CancellationToken cancellationToken = default);
    Task<ProjectDetailDto?> GetProjectAsync(Guid id, CancellationToken cancellationToken = default);
}

public class ProjectReadService : IProjectReadService
{
    private readonly IAppDbContext _dbContext;

    public ProjectReadService(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<ProjectSummaryDto>> GetProjectsAsync(CancellationToken cancellationToken = default)
    {
        var projects = await _dbContext.Projects
            .AsNoTracking()
            .Include(p => p.Account)
            .Include(p => p.WorkItems)
            .Include(p => p.Labels).ThenInclude(l => l.Label)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        return projects.Select(p => new ProjectSummaryDto(
                p.Id,
                p.Name,
                p.Status,
                p.Account?.Name ?? string.Empty,
                p.StartDate,
                p.EndDate,
                p.Budget,
                p.WorkItems.Count(w => w.Status is WorkItemStatus.Planned or WorkItemStatus.InProgress or WorkItemStatus.Blocked),
                p.Labels.OrderBy(l => l.Label?.Name).Select(l => l.Label?.Name ?? string.Empty)))
            .ToList();
    }

    public async Task<ProjectDetailDto?> GetProjectAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var project = await _dbContext.Projects
            .AsNoTracking()
            .Include(p => p.Account)
            .Include(p => p.Stages)
            .Include(p => p.Members).ThenInclude(m => m.User)
            .Include(p => p.WorkItems).ThenInclude(w => w.Assignee)
            .Include(p => p.WorkItems).ThenInclude(w => w.TimeLogs)
            .Include(p => p.WorkItems).ThenInclude(w => w.Labels).ThenInclude(l => l.Label)
            .Include(p => p.Labels).ThenInclude(l => l.Label)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (project is null)
        {
            return null;
        }

        var workItems = project.WorkItems
            .OrderBy(w => w.DueDate ?? w.CreatedAt)
            .Select(w => new WorkItemSummaryDto(
                w.Id,
                w.Title,
                w.Type.ToString(),
                w.Status.ToString(),
                w.Priority.ToString(),
                w.AssigneeId,
                w.Assignee?.DisplayName,
                w.CreatedAt,
                w.DueDate,
                w.Labels.OrderBy(l => l.Label?.Name).Select(l => l.Label?.Name ?? string.Empty),
                w.TimeLogs.Sum(t => ((t.End ?? t.Start) - t.Start).TotalHours)));

        return new ProjectDetailDto(
            project.Id,
            project.Name,
            project.Code,
            project.Status,
            project.StartDate,
            project.EndDate,
            project.Budget,
            project.Stages.OrderBy(s => s.Order).Select(s => new ProjectStageDto(s.Id, s.Name, s.Order)),
            project.Members.OrderBy(m => m.User?.DisplayName).Select(m => new ProjectMemberDto(m.Id, m.User?.DisplayName ?? string.Empty, m.Role)),
            workItems,
            project.Labels.OrderBy(l => l.Label?.Name).Select(l => l.Label?.Name ?? string.Empty));
    }
}
