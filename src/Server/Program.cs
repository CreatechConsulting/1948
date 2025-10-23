using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WorkManagement.Application.Dto;
using WorkManagement.Application.Services;
using WorkManagement.Infrastructure;
using WorkManagement.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration)
          .Enrich.FromLogContext()
          .WriteTo.Console();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("default", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

builder.Services.AddWorkManagementInfrastructure(options =>
{
    options.UseInMemoryDatabase("work-management");
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureCreatedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseCors("default");

app.MapGet("/api/dashboard", async (HttpContext context, IDashboardService dashboardService) =>
{
    Guid? currentUserId = null;
    if (context.Request.Headers.TryGetValue("X-User-Id", out var header) && Guid.TryParse(header, out var parsed))
    {
        currentUserId = parsed;
    }

    var summary = await dashboardService.GetDashboardAsync(currentUserId);
    return Results.Ok(summary);
})
.WithName("GetDashboard")
.WithOpenApi();

app.MapGet("/api/accounts", async (IAccountReadService service) =>
{
    var accounts = await service.GetAccountsAsync();
    return Results.Ok(accounts);
})
.WithName("GetAccounts")
.Produces<IEnumerable<AccountSummaryDto>>(StatusCodes.Status200OK)
.WithOpenApi();

app.MapGet("/api/accounts/{id:guid}", async Task<Results<Ok<AccountDetailDto>, NotFound>> (Guid id, IAccountReadService service) =>
{
    var account = await service.GetAccountAsync(id);
    return account is null ? TypedResults.NotFound() : TypedResults.Ok(account);
})
.WithName("GetAccount")
.Produces<AccountDetailDto>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithOpenApi();

app.MapGet("/api/projects", async (IProjectReadService service) =>
{
    var projects = await service.GetProjectsAsync();
    return Results.Ok(projects);
})
.WithName("GetProjects")
.Produces<IEnumerable<ProjectSummaryDto>>(StatusCodes.Status200OK)
.WithOpenApi();

app.MapGet("/api/projects/{id:guid}", async Task<Results<Ok<ProjectDetailDto>, NotFound>> (Guid id, IProjectReadService service) =>
{
    var project = await service.GetProjectAsync(id);
    return project is null ? TypedResults.NotFound() : TypedResults.Ok(project);
})
.WithName("GetProject")
.Produces<ProjectDetailDto>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithOpenApi();

app.MapGet("/api/work-items/{id:guid}", async Task<Results<Ok<WorkItemDetailDto>, NotFound>> (Guid id, IWorkItemReadService service) =>
{
    var workItem = await service.GetWorkItemAsync(id);
    return workItem is null ? TypedResults.NotFound() : TypedResults.Ok(workItem);
})
.WithName("GetWorkItem")
.Produces<WorkItemDetailDto>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithOpenApi();

app.Run();
