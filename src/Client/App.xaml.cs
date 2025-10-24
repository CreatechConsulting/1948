using Microsoft.Maui.Controls;

namespace WorkManagement.Client;

public partial class App : Microsoft.Maui.Controls.Application
{
    public App()
    {
        InitializeComponent();
    }
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new NavigationPage(new MainPage())
        {
            BarBackgroundColor = Color.FromArgb("#ffc107"),
            BarTextColor = Colors.White
        });
    }

}
