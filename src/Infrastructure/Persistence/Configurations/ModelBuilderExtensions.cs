using System;
using Microsoft.EntityFrameworkCore;
using WorkManagement.Domain.Entities;

namespace WorkManagement.Infrastructure.Persistence.Configurations;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        var adminUser = new User
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            DisplayName = "Avery Admin",
            Email = "avery.admin@example.com",
            Role = UserRoles.Admin
        };

        var projectManager = new User
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            DisplayName = "Parker Project",
            Email = "parker.project@example.com",
            Role = UserRoles.ProjectManager
        };

        var contributor = new User
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            DisplayName = "Casey Contributor",
            Email = "casey.contributor@example.com",
            Role = UserRoles.Contributor
        };

        var labels = new[]
        {
            new Label { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "High Priority", Color = "#FF4C51" },
            new Label { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "Backlog", Color = "#8884FF" },
            new Label { Id = Guid.Parse("66666666-6666-6666-6666-666666666666"), Name = "In Discovery", Color = "#F7B801" }
        };

        var accountId = Guid.Parse("77777777-7777-7777-7777-777777777777");
        var discoveryProjectId = Guid.Parse("88888888-8888-8888-8888-888888888888");
        var deliveryProjectId = Guid.Parse("99999999-9999-9999-9999-999999999999");

        var account = new Account
        {
            Id = accountId,
            Name = "Aurora Manufacturing",
            Code = "AUR-001",
            Description = "Industrial equipment manufacturer",
            Type = AccountType.Customer,
            Status = "Active"
        };

        var contact = new Contact
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            AccountId = accountId,
            FullName = "Jamie Rivera",
            Email = "jamie.rivera@aurora.example.com",
            Phone = "+1-555-0100"
        };

        var accountTag = new AccountTag
        {
            Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            AccountId = accountId,
            Value = "Strategic"
        };

        var discoveryProject = new Project
        {
            Id = discoveryProjectId,
            AccountId = accountId,
            Name = "Aurora Discovery",
            Code = "AUR-DSC",
            Status = ProjectStatus.Active,
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30)),
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
            Budget = 150000M
        };

        var deliveryProject = new Project
        {
            Id = deliveryProjectId,
            AccountId = accountId,
            Name = "Aurora Delivery",
            Code = "AUR-DLV",
            Status = ProjectStatus.Planned,
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(14)),
            Budget = 320000M
        };

        var discoveryStage = new ProjectStage
        {
            Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
            ProjectId = discoveryProjectId,
            Name = "Discovery",
            Order = 1
        };

        var implementationStage = new ProjectStage
        {
            Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
            ProjectId = deliveryProjectId,
            Name = "Implementation",
            Order = 1
        };

        var projectMember = new ProjectMember
        {
            Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
            ProjectId = discoveryProjectId,
            UserId = projectManager.Id,
            Role = UserRoles.ProjectManager
        };

        var workItemId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");
        var workItem = new WorkItem
        {
            Id = workItemId,
            ProjectId = discoveryProjectId,
            Title = "Conduct stakeholder interviews",
            Description = "Schedule and interview key Aurora stakeholders to gather requirements.",
            Type = WorkItemType.Task,
            Status = WorkItemStatus.InProgress,
            Priority = WorkItemPriority.High,
            AssigneeId = contributor.Id,
            CreatedAt = DateTimeOffset.UtcNow.AddDays(-7),
            DueDate = DateTimeOffset.UtcNow.AddDays(3)
        };

        var workItemLabel = new WorkItemLabel
        {
            WorkItemId = workItemId,
            LabelId = labels[0].Id
        };

        var comment = new Comment
        {
            Id = Guid.Parse("12121212-1212-1212-1212-121212121212"),
            WorkItemId = workItemId,
            AuthorId = projectManager.Id,
            Body = "Draft interview guide is ready for review.",
            CreatedAt = DateTimeOffset.UtcNow.AddDays(-2)
        };

        var timeLog = new TimeLog
        {
            Id = Guid.Parse("13131313-1313-1313-1313-131313131313"),
            WorkItemId = workItemId,
            UserId = contributor.Id,
            Start = DateTimeOffset.UtcNow.AddDays(-1).AddHours(-2),
            End = DateTimeOffset.UtcNow.AddDays(-1).AddHours(1),
            Notes = "Interviewed procurement lead."
        };

        modelBuilder.Entity<User>().HasData(adminUser, projectManager, contributor);
        modelBuilder.Entity<Label>().HasData(labels);
        modelBuilder.Entity<Account>().HasData(account);
        modelBuilder.Entity<Contact>().HasData(contact);
        modelBuilder.Entity<AccountTag>().HasData(accountTag);
        modelBuilder.Entity<Project>().HasData(discoveryProject, deliveryProject);
        modelBuilder.Entity<ProjectStage>().HasData(discoveryStage, implementationStage);
        modelBuilder.Entity<ProjectMember>().HasData(projectMember);
        modelBuilder.Entity<WorkItem>().HasData(workItem);
        modelBuilder.Entity<WorkItemLabel>().HasData(workItemLabel);
        modelBuilder.Entity<Comment>().HasData(comment);
        modelBuilder.Entity<TimeLog>().HasData(timeLog);
    }
}
