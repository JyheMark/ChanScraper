using System.Net.Http;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ChanScraper.Gui.ViewModels;
using ChanScraper.Gui.Views;
using ChanScraper.Library.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChanScraper.Gui;

public class App : Application
{
    public App()
    {
        AppHost = Host
            .CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton(new HttpClient());
                services.AddChanScraperServices();
            })
            .Build();

        var runProcess = AppHost.RunAsync();
    }

    public IHost AppHost { get; }

    public static T GetService<T>() where T : class
    {
        return (Current as App).AppHost.Services.GetService<T>();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };

        base.OnFrameworkInitializationCompleted();
    }
}
