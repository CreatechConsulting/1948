using System;
using System.Collections.Generic;

namespace WorkManagement.Domain.Entities;

public class Comment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid WorkItemId { get; set; }
    public WorkItem? WorkItem { get; set; }
    public Guid AuthorId { get; set; }
    public User? Author { get; set; }
    public string Body { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public class TimeLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid WorkItemId { get; set; }
    public WorkItem? WorkItem { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset? End { get; set; }
    public string? Notes { get; set; }
    public TimeSpan Duration => End.HasValue ? End.Value - Start : TimeSpan.Zero;
}

public class Attachment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid WorkItemId { get; set; }
    public WorkItem? WorkItem { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = "application/octet-stream";
    public long Size { get; set; }
    public string StoragePath { get; set; } = string.Empty;
    public DateTimeOffset UploadedAt { get; set; } = DateTimeOffset.UtcNow;
    public Guid UploadedById { get; set; }
    public User? UploadedBy { get; set; }
}

public class Label
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = "#FFFFFF";
    public ICollection<ProjectLabel> Projects { get; set; } = new List<ProjectLabel>();
    public ICollection<WorkItemLabel> WorkItems { get; set; } = new List<WorkItemLabel>();
}

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = UserRoles.Contributor;
    public bool IsActive { get; set; } = true;
    public ICollection<ProjectMember> Projects { get; set; } = new List<ProjectMember>();
    public ICollection<WorkItem> AssignedWorkItems { get; set; } = new List<WorkItem>();
}

public static class UserRoles
{
    public const string Admin = "Admin";
    public const string ProjectManager = "Project Manager";
    public const string Contributor = "Contributor";
    public const string Viewer = "Viewer";
}
