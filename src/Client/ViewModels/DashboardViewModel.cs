using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using WorkManagement.Application.Dto;
using WorkManagement.Shared.Contracts;

namespace WorkManagement.Client.ViewModels;

public class DashboardViewModel : ObservableObject
{
    private readonly IWorkManagementApi _api;

    public ObservableCollection<KpiCardViewModel> Kpis { get; } = new();
    public ObservableCollection<MyWorkItemViewModel> MyWork { get; } = new();
    public ObservableCollection<ActivityFeedItemViewModel> Activity { get; } = new();

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public DashboardViewModel(IWorkManagementApi api)
    {
        _api = api;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            var summary = await _api.GetDashboardAsync(userId: Guid.Parse("33333333-3333-3333-3333-333333333333"), cancellationToken);

            UpdateKpis(summary.Kpis);
            UpdateMyWork(summary.MyWork);
            UpdateActivity(summary.RecentActivity);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void UpdateKpis(IEnumerable<KpiCardDto> kpis)
    {
        Kpis.Clear();
        foreach (var kpi in kpis)
        {
            var accent = kpi.Accent switch
            {
                "accent-work" => "#1DB954",
                "accent-hours" => "#F7B801",
                "accent-projects" => "#4CB0F9",
                _ => "#2A2A2A"
            };

            var numericValue = TryParseDouble(kpi.Value);
            Kpis.Add(new KpiCardViewModel(kpi.Title, kpi.Value, kpi.Trend, accent, numericValue));
        }
    }

    private void UpdateMyWork(IEnumerable<MyWorkItemDto> workItems)
    {
        MyWork.Clear();
        foreach (var item in workItems)
        {
            MyWork.Add(new MyWorkItemViewModel(item.WorkItemId, item.Title, item.ProjectName, item.Status, item.DueDate, item.LoggedHours));
        }
    }

    private void UpdateActivity(IEnumerable<ActivityFeedItemDto> activity)
    {
        Activity.Clear();
        foreach (var item in activity)
        {
            Activity.Add(new ActivityFeedItemViewModel(item.Id, item.Description, item.CreatedAt, item.Category));
        }
    }

    private static double TryParseDouble(string value)
    {
        if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed))
        {
            return parsed;
        }

        if (double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out parsed))
        {
            return parsed;
        }

        return 0d;
    }
}

public record KpiCardViewModel(string Title, string Value, string Trend, string AccentColor, double NumericValue);

public record MyWorkItemViewModel(Guid Id, string Title, string ProjectName, string Status, DateTimeOffset? DueDate, double LoggedHours);

public record ActivityFeedItemViewModel(Guid Id, string Description, DateTimeOffset CreatedAt, string Category);
