using Akka.Actor;
using Akka.Hosting;
using ChanScraper.Library.Actors;
using ChanScraper.Library.Actors.Messages;
using ChanScraper.Library.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

HostApplicationBuilder builder = Host.CreateApplicationBuilder();

builder.Logging.ClearProviders();

builder.Services.AddSingleton(new HttpClient());
builder.Services.AddChanScraperServices();

IHost app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();
var scraperParentActor = app.Services.GetRequiredService<IRequiredActor<ChanScraperEntryActor>>();

try
{
    var downloadPath = string.Empty;

    while (true)
    {
        downloadPath = GetInput("Enter download path");

        if (!DownloadPathIsValid(downloadPath))
            Console.WriteLine("Download path invalid");
        else break;
    }

    Task runTask = app.RunAsync();

    scraperParentActor.ActorRef.Tell(new SetDownloadLocation(downloadPath!), ActorRefs.NoSender);

    while (true)
    {
        var boardNameInput = GetInput("Enter board");
        var threadIdInput = GetInput("Enter thread Id");
        var threadId = int.Parse(threadIdInput);
        
        scraperParentActor.ActorRef.Tell(new StartWatchingThread(boardNameInput, threadId));
    }
}
catch (Exception ex)
{
    logger.LogCritical(ex, "Fatal error");
}

bool DownloadPathIsValid(string? path)
{
    if (string.IsNullOrWhiteSpace(path))
        return false;

    return Path.Exists(path);
}

string? GetInput(string msg)
{
    Console.Clear();
    Console.Write($"{msg}: ");
    return Console.ReadLine();
}