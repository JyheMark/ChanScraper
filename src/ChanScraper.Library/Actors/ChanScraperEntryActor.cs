using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Event;
using ChanScraper.Library.Actors.Messages;

namespace ChanScraper.Library.Actors;

public sealed class ChanScraperEntryActor : ReceiveActor
{
    private readonly ILoggingAdapter _logger;
    private string? _downloadLocation;

    public ChanScraperEntryActor()
    {
        _logger = Context.GetLogger();
        _logger.Info("Starting actor");

        Receive<SetDownloadLocation>(msg =>
        {
            _downloadLocation = msg.Path;
            _logger.Info($"Download path set to {_downloadLocation}");
            
            Become(Ready);
        });
    }

    private void Ready()
    {
        Receive<WatchThread>(msg =>
        {
            var scraperActor = Context.ActorOf(DependencyResolver.For(Context.System).Props<ThreadScraperActor>());
            scraperActor.Tell(new StartScraping(msg.Board, msg.ThreadId, _downloadLocation), Self);
        });
    }
}
