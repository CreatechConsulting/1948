using System.Collections.Generic;

namespace WorkManagement.Application.Dto;

public record ProjectSummaryDto(
    Guid Id,
    string Name,
    string Status,
    string AccountName,
    DateOnly? StartDate,
    DateOnly? EndDate,
    decimal? Budget,
    int ActiveWorkItems,
    IEnumerable<string> Labels);

public record ProjectDetailDto(
    Guid Id,
    string Name,
    string? Code,
    string Status,
    DateOnly? StartDate,
    DateOnly? EndDate,
    decimal? Budget,
    IEnumerable<ProjectStageDto> Stages,
    IEnumerable<ProjectMemberDto> Members,
    IEnumerable<WorkItemSummaryDto> WorkItems,
    IEnumerable<string> Labels);

public record ProjectStageDto(Guid Id, string Name, int Order);

public record ProjectMemberDto(Guid Id, string DisplayName, string Role);
