using System;
using Microsoft.EntityFrameworkCore;
using WorkManagement.Application.Abstractions;
using WorkManagement.Domain.Entities;
using WorkManagement.Infrastructure.Persistence.Configurations;

namespace WorkManagement.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<WorkItem> WorkItems => Set<WorkItem>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Label> Labels => Set<Label>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<TimeLog> TimeLogs => Set<TimeLog>();
    public DbSet<Attachment> Attachments => Set<Attachment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>(entity =>
        {
            entity.Property(a => a.Name).IsRequired().HasMaxLength(200);
            entity.Property(a => a.Code).HasMaxLength(50);
            entity.Property(a => a.Status).HasMaxLength(50);
            entity.HasMany(a => a.Projects).WithOne(p => p.Account).HasForeignKey(p => p.AccountId);
            entity.HasMany(a => a.Tags).WithOne(t => t.Account).HasForeignKey(t => t.AccountId);
            entity.HasMany(a => a.Contacts).WithOne(c => c.Account).HasForeignKey(c => c.AccountId);
        });

        modelBuilder.Entity<AccountTag>(entity =>
        {
            entity.Property(t => t.Value).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Code).HasMaxLength(50);
            entity.Property(p => p.Status).HasMaxLength(50);
            entity.HasMany(p => p.Stages).WithOne(s => s.Project).HasForeignKey(s => s.ProjectId);
            entity.HasMany(p => p.WorkItems).WithOne(w => w.Project).HasForeignKey(w => w.ProjectId);
            entity.HasMany(p => p.Members).WithOne(m => m.Project).HasForeignKey(m => m.ProjectId);
            entity.HasMany(p => p.Labels).WithOne(l => l.Project).HasForeignKey(l => l.ProjectId);
        });

        modelBuilder.Entity<ProjectStage>(entity =>
        {
            entity.Property(s => s.Name).IsRequired().HasMaxLength(200);
            entity.HasIndex(s => new { s.ProjectId, s.Order }).IsUnique();
        });

        modelBuilder.Entity<ProjectMember>(entity =>
        {
            entity.HasOne(m => m.User).WithMany(u => u.Projects).HasForeignKey(m => m.UserId);
        });

        modelBuilder.Entity<ProjectLabel>(entity =>
        {
            entity.HasKey(l => new { l.ProjectId, l.LabelId });
            entity.HasOne(l => l.Label).WithMany(l => l.Projects).HasForeignKey(l => l.LabelId);
        });

        modelBuilder.Entity<WorkItem>(entity =>
        {
            entity.Property(w => w.Title).IsRequired().HasMaxLength(300);
            entity.HasOne(w => w.Parent).WithMany(w => w.Children).HasForeignKey(w => w.ParentId);
            entity.HasOne(w => w.Assignee).WithMany(u => u.AssignedWorkItems).HasForeignKey(w => w.AssigneeId);
            entity.HasMany(w => w.Comments).WithOne(c => c.WorkItem).HasForeignKey(c => c.WorkItemId);
            entity.HasMany(w => w.Attachments).WithOne(a => a.WorkItem).HasForeignKey(a => a.WorkItemId);
            entity.HasMany(w => w.TimeLogs).WithOne(t => t.WorkItem).HasForeignKey(t => t.WorkItemId);
            entity.HasMany(w => w.Labels).WithOne(l => l.WorkItem).HasForeignKey(l => l.WorkItemId);
        });

        modelBuilder.Entity<WorkItemLabel>(entity =>
        {
            entity.HasKey(l => new { l.WorkItemId, l.LabelId });
            entity.HasOne(l => l.Label).WithMany(l => l.WorkItems).HasForeignKey(l => l.LabelId);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.Property(c => c.Body).IsRequired();
            entity.HasOne(c => c.Author).WithMany().HasForeignKey(c => c.AuthorId);
        });

        modelBuilder.Entity<TimeLog>(entity =>
        {
            entity.HasOne(t => t.User).WithMany().HasForeignKey(t => t.UserId);
        });

        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.Property(a => a.FileName).IsRequired().HasMaxLength(260);
            entity.Property(a => a.ContentType).HasMaxLength(150);
            entity.HasOne(a => a.UploadedBy).WithMany().HasForeignKey(a => a.UploadedById);
        });

        modelBuilder.Entity<Label>(entity =>
        {
            entity.Property(l => l.Name).IsRequired().HasMaxLength(100);
            entity.Property(l => l.Color).HasMaxLength(20);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.DisplayName).IsRequired().HasMaxLength(200);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
            entity.Property(u => u.Role).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Seed();
    }
}
