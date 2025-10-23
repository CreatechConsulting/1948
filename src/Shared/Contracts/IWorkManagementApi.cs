using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using WorkManagement.Application.Dto;

namespace WorkManagement.Shared.Contracts;

public interface IWorkManagementApi
{
    [Get("/api/dashboard")]
    Task<DashboardSummaryDto> GetDashboardAsync([Header("X-User-Id")] Guid? userId = null, CancellationToken cancellationToken = default);

    [Get("/api/accounts")]
    Task<IReadOnlyList<AccountSummaryDto>> GetAccountsAsync(CancellationToken cancellationToken = default);

    [Get("/api/accounts/{id}")]
    Task<AccountDetailDto> GetAccountAsync(Guid id, CancellationToken cancellationToken = default);

    [Get("/api/projects")]
    Task<IReadOnlyList<ProjectSummaryDto>> GetProjectsAsync(CancellationToken cancellationToken = default);

    [Get("/api/projects/{id}")]
    Task<ProjectDetailDto> GetProjectAsync(Guid id, CancellationToken cancellationToken = default);

    [Get("/api/work-items/{id}")]
    Task<WorkItemDetailDto> GetWorkItemAsync(Guid id, CancellationToken cancellationToken = default);
}
