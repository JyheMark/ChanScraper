using Akka.Hosting;
using ChanScraper.ChanApi;
using ChanScraper.Library.Actors;
using Microsoft.Extensions.DependencyInjection;

namespace ChanScraper.Library.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddChanScraperServices(this IServiceCollection services)
    {
        services.AddTransient<IChanClient, ChanClient>();
        
        services.AddAkka("chan-scraper-system", builder =>
        {
            builder
                .ConfigureLoggers(config => config.AddLoggerFactory())
                .WithActors((system, registry, resolver) =>
                {
                    var parentActor = system.ActorOf(resolver.Props<ChanScraperEntryActor>(), "parent-actor");
                    registry.TryRegister<ChanScraperEntryActor>(parentActor);
                });
        });
    }
}
