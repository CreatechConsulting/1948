using Microsoft.EntityFrameworkCore;
using WorkManagement.Application.Abstractions;
using WorkManagement.Application.Dto;
using WorkManagement.Domain.Entities;

namespace WorkManagement.Application.Services;

public interface IAccountReadService
{
    Task<IReadOnlyList<AccountSummaryDto>> GetAccountsAsync(CancellationToken cancellationToken = default);
    Task<AccountDetailDto?> GetAccountAsync(Guid id, CancellationToken cancellationToken = default);
}

public class AccountReadService : IAccountReadService
{
    private readonly IAppDbContext _dbContext;

    public AccountReadService(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<AccountSummaryDto>> GetAccountsAsync(CancellationToken cancellationToken = default)
    {
        var accounts = await _dbContext.Accounts
            .AsNoTracking()
            .Include(a => a.Projects)
                .ThenInclude(p => p.WorkItems)
            .Include(a => a.Tags)
            .OrderBy(a => a.Name)
            .ToListAsync(cancellationToken);

        return accounts.Select(a => new AccountSummaryDto(
                a.Id,
                a.Name,
                a.Type.ToString(),
                a.Status,
                a.Projects.Count,
                a.Projects.SelectMany(p => p.WorkItems).Count(w => w.Status is WorkItemStatus.Planned or WorkItemStatus.InProgress or WorkItemStatus.Blocked),
                a.Tags.OrderBy(t => t.Value).Select(t => t.Value)))
            .ToList();
    }

    public async Task<AccountDetailDto?> GetAccountAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var account = await _dbContext.Accounts
            .AsNoTracking()
            .Include(a => a.Projects).ThenInclude(p => p.WorkItems)
            .Include(a => a.Projects).ThenInclude(p => p.Labels).ThenInclude(pl => pl.Label)
            .Include(a => a.Tags)
            .Include(a => a.Contacts)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (account is null)
        {
            return null;
        }

        var projects = account.Projects
            .OrderBy(p => p.Name)
            .Select(p => new ProjectSummaryDto(
                p.Id,
                p.Name,
                p.Status,
                account.Name,
                p.StartDate,
                p.EndDate,
                p.Budget,
                p.WorkItems.Count(w => w.Status is WorkItemStatus.Planned or WorkItemStatus.InProgress or WorkItemStatus.Blocked),
                p.Labels.OrderBy(l => l.Label?.Name).Select(l => l.Label?.Name ?? string.Empty)));

        return new AccountDetailDto(
            account.Id,
            account.Name,
            account.Type.ToString(),
            account.Description,
            account.Status,
            projects,
            account.Contacts.OrderBy(c => c.FullName).Select(c => new ContactDto(c.Id, c.FullName, c.Email, c.Phone)),
            account.Tags.OrderBy(t => t.Value).Select(t => t.Value));
    }
}
