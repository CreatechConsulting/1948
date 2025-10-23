using Microsoft.Maui.Controls;

namespace WorkManagement.Client;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new MainPage();
    }
}
