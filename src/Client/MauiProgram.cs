using System;
using CommunityToolkit.Maui;
using DevExpress.Blazor;
using DevExpress.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Refit;
using WorkManagement.Client.ViewModels;
using WorkManagement.Shared.Contracts;

namespace WorkManagement.Client;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseDevExpress()
            .UseMauiCommunityToolkit();

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddDevExpressBlazor();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services
            .AddRefitClient<IWorkManagementApi>()
            .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://localhost:7162"));

        builder.Services.AddScoped<DashboardViewModel>();

        return builder.Build();
    }
}
