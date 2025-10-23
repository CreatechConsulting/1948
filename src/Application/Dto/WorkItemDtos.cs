using System.Collections.Generic;

namespace WorkManagement.Application.Dto;

public record WorkItemSummaryDto(
    Guid Id,
    string Title,
    string Type,
    string Status,
    string Priority,
    Guid? AssigneeId,
    string? Assignee,
    DateTimeOffset CreatedAt,
    DateTimeOffset? DueDate,
    IEnumerable<string> Labels,
    double LoggedHours);

public record WorkItemDetailDto(
    Guid Id,
    string Title,
    string? Description,
    string Type,
    string Status,
    string Priority,
    Guid? AssigneeId,
    string? Assignee,
    DateTimeOffset CreatedAt,
    DateTimeOffset? DueDate,
    DateTimeOffset? CompletedAt,
    IEnumerable<CommentDto> Comments,
    IEnumerable<AttachmentDto> Attachments,
    IEnumerable<TimeLogDto> TimeLogs,
    IEnumerable<string> Labels);

public record CommentDto(Guid Id, Guid AuthorId, string Author, string Body, DateTimeOffset CreatedAt);

public record AttachmentDto(Guid Id, string FileName, string ContentType, long Size, DateTimeOffset UploadedAt, Guid UploadedById, string UploadedBy);

public record TimeLogDto(Guid Id, Guid UserId, string User, DateTimeOffset Start, DateTimeOffset? End, double DurationHours, string? Notes);
