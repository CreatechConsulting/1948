using System;
using System.Collections.Generic;

namespace WorkManagement.Domain.Entities;

public class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public Guid AccountId { get; set; }
    public Account? Account { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public decimal? Budget { get; set; }
    public string Status { get; set; } = ProjectStatus.Planned;
    public ICollection<ProjectStage> Stages { get; set; } = new List<ProjectStage>();
    public ICollection<WorkItem> WorkItems { get; set; } = new List<WorkItem>();
    public ICollection<ProjectMember> Members { get; set; } = new List<ProjectMember>();
    public ICollection<ProjectLabel> Labels { get; set; } = new List<ProjectLabel>();
}

public static class ProjectStatus
{
    public const string Planned = "Planned";
    public const string Active = "Active";
    public const string OnHold = "On Hold";
    public const string Completed = "Completed";
    public const string Archived = "Archived";
}

public class ProjectStage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
}

public class ProjectMember
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string Role { get; set; } = string.Empty;
}

public class ProjectLabel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    public Guid LabelId { get; set; }
    public Label? Label { get; set; }
}
