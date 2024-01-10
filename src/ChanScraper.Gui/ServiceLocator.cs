using System;
using Akka.Actor;

namespace ChanScraper.Gui;

internal static class ServiceLocator
{
    public static IServiceProvider ServiceProvider { get; set; }
    public static IActorRef ChanEntryActor { get; set; }


    public static T? GetService<T>() where T : class
    {
        ArgumentNullException.ThrowIfNull(ServiceProvider);
        return ServiceProvider.GetService(typeof(T)) as T;
    }
}
