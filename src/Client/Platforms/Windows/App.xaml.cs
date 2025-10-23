using Microsoft.UI.Xaml;

namespace WorkManagement.Client.WinUI;

public partial class App : MauiWinUIApplication
{
    public App()
    {
        this.InitializeComponent();
    }

    protected override MauiApp CreateMauiApp() => WorkManagement.Client.MauiProgram.CreateMauiApp();
}
