using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WorkManagement.Application.Abstractions;
using WorkManagement.Application.Services;
using WorkManagement.Infrastructure.Persistence;

namespace WorkManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddWorkManagementInfrastructure(this IServiceCollection services, Action<DbContextOptionsBuilder>? options = null)
    {
        if (options != null)
        {
            services.AddDbContext<AppDbContext>(options);
        }
        else
        {
            services.AddDbContext<AppDbContext>(builder => builder.UseInMemoryDatabase("work-management"));
        }
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        services.AddScoped<IAccountReadService, AccountReadService>();
        services.AddScoped<IProjectReadService, ProjectReadService>();
        services.AddScoped<IWorkItemReadService, WorkItemReadService>();
        services.AddScoped<IDashboardService, DashboardService>();

        return services;
    }
}
