using Microsoft.EntityFrameworkCore;
using WorkManagement.Application.Abstractions;
using WorkManagement.Application.Dto;

namespace WorkManagement.Application.Services;

public interface IWorkItemReadService
{
    Task<WorkItemDetailDto?> GetWorkItemAsync(Guid id, CancellationToken cancellationToken = default);
}

public class WorkItemReadService : IWorkItemReadService
{
    private readonly IAppDbContext _dbContext;

    public WorkItemReadService(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<WorkItemDetailDto?> GetWorkItemAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var workItem = await _dbContext.WorkItems
            .AsNoTracking()
            .Include(w => w.Assignee)
            .Include(w => w.Comments).ThenInclude(c => c.Author)
            .Include(w => w.Attachments).ThenInclude(a => a.UploadedBy)
            .Include(w => w.TimeLogs).ThenInclude(t => t.User)
            .Include(w => w.Labels).ThenInclude(l => l.Label)
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

        if (workItem is null)
        {
            return null;
        }

        return new WorkItemDetailDto(
            workItem.Id,
            workItem.Title,
            workItem.Description,
            workItem.Type.ToString(),
            workItem.Status.ToString(),
            workItem.Priority.ToString(),
            workItem.AssigneeId,
            workItem.Assignee?.DisplayName,
            workItem.CreatedAt,
            workItem.DueDate,
            workItem.CompletedAt,
            workItem.Comments.OrderBy(c => c.CreatedAt).Select(c => new CommentDto(c.Id, c.AuthorId, c.Author?.DisplayName ?? string.Empty, c.Body, c.CreatedAt)),
            workItem.Attachments.OrderByDescending(a => a.UploadedAt).Select(a => new AttachmentDto(a.Id, a.FileName, a.ContentType, a.Size, a.UploadedAt, a.UploadedById, a.UploadedBy?.DisplayName ?? string.Empty)),
            workItem.TimeLogs.OrderByDescending(t => t.Start).Select(t => new TimeLogDto(t.Id, t.UserId, t.User?.DisplayName ?? string.Empty, t.Start, t.End, ((t.End ?? t.Start) - t.Start).TotalHours, t.Notes)),
            workItem.Labels.OrderBy(l => l.Label?.Name).Select(l => l.Label?.Name ?? string.Empty));
    }
}
