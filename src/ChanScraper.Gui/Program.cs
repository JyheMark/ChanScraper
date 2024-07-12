using System;
using System.Net.Http;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.DependencyInjection;
using Avalonia;
using Avalonia.ReactiveUI;
using ChanScraper.ChanApi;
using ChanScraper.Library.Actors;
using Microsoft.Extensions.DependencyInjection;

namespace ChanScraper.Gui;

internal sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static async Task Main(string[] args)
    {
        var servicesBuilder = new ServiceCollection();
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");
        servicesBuilder.AddSingleton(httpClient);
        servicesBuilder.AddTransient<IChanClient, ChanClient>();
        servicesBuilder.BuildServiceProvider();
        
        var services = servicesBuilder.BuildServiceProvider();

        var dependencyResolver = DependencyResolverSetup.Create(services);
        var actorSystem = ActorSystem.Create("chanscraper-system", BootstrapSetup.Create().And(dependencyResolver));
        var chanEntryActor = actorSystem.ActorOf(Props.Create<ChanScraperEntryActor>(), "scraper-parent-actor");
        
        ServiceLocator.ServiceProvider = services;
        ServiceLocator.ChanEntryActor = chanEntryActor;

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
    }
}
