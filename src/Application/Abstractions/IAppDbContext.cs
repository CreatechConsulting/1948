using Microsoft.EntityFrameworkCore;
using WorkManagement.Domain.Entities;

namespace WorkManagement.Application.Abstractions;

public interface IAppDbContext
{
    DbSet<Account> Accounts { get; }
    DbSet<Project> Projects { get; }
    DbSet<WorkItem> WorkItems { get; }
    DbSet<User> Users { get; }
    DbSet<Label> Labels { get; }
    DbSet<Comment> Comments { get; }
    DbSet<TimeLog> TimeLogs { get; }
    DbSet<Attachment> Attachments { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
