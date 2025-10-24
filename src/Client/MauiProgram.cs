using CommunityToolkit.Maui;
using Refit;
using WorkManagement.Client.ViewModels;
using Microsoft.Extensions.Logging; // <-- Add this using directive
using Microsoft.Extensions.Logging.Debug; // <-- Add this using directive

namespace WorkManagement.Client;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder 
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit();

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddDevExpressBlazor();

#if DEBUG
        builder.Logging.AddDebug();
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        builder.Services
            .AddRefitClient<WorkManagement.Shared.Contracts.IWorkManagementApi>()
            .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://localhost:7162"));

        builder.Services.AddScoped<DashboardViewModel>();

        return builder.Build();
    }
}
