using System;
using System.Collections.Generic;

namespace WorkManagement.Domain.Entities;

public class WorkItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public WorkItemType Type { get; set; } = WorkItemType.Task;
    public WorkItemStatus Status { get; set; } = WorkItemStatus.Planned;
    public WorkItemPriority Priority { get; set; } = WorkItemPriority.Normal;
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    public Guid? ParentId { get; set; }
    public WorkItem? Parent { get; set; }
    public ICollection<WorkItem> Children { get; set; } = new List<WorkItem>();
    public Guid? AssigneeId { get; set; }
    public User? Assignee { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    public ICollection<WorkItemLabel> Labels { get; set; } = new List<WorkItemLabel>();
    public ICollection<TimeLog> TimeLogs { get; set; } = new List<TimeLog>();
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? DueDate { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
}

public enum WorkItemType
{
    Task,
    Ticket,
    Bug
}

public enum WorkItemStatus
{
    Planned,
    InProgress,
    Blocked,
    Completed,
    Archived
}

public enum WorkItemPriority
{
    Low,
    Normal,
    High,
    Critical
}

public class WorkItemLabel
{
    public Guid WorkItemId { get; set; }
    public WorkItem? WorkItem { get; set; }
    public Guid LabelId { get; set; }
    public Label? Label { get; set; }
}
