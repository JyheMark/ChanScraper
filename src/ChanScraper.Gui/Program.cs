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
        servicesBuilder.AddSingleton(new HttpClient());
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
