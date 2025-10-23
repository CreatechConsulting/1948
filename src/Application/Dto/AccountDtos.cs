using System.Collections.Generic;

namespace WorkManagement.Application.Dto;

public record AccountSummaryDto(
    Guid Id,
    string Name,
    string Type,
    string? Status,
    int ProjectCount,
    int ActiveWorkItems,
    IEnumerable<string> Tags);

public record AccountDetailDto(
    Guid Id,
    string Name,
    string Type,
    string? Description,
    string? Status,
    IEnumerable<ProjectSummaryDto> Projects,
    IEnumerable<ContactDto> Contacts,
    IEnumerable<string> Tags);

public record ContactDto(Guid Id, string FullName, string? Email, string? Phone);
