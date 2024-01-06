using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Event;
using ChanScraper.Library.Actors.Messages;

namespace ChanScraper.Library.Actors;

public sealed class ChanScraperEntryActor : ReceiveActor
{
    private readonly ILoggingAdapter _logger;
    private readonly Dictionary<(string, int), IActorRef> _threadWatcherActors;
    private string? _downloadLocation;

    public ChanScraperEntryActor()
    {
        _threadWatcherActors = new Dictionary<(string, int), IActorRef>();
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
        Receive<StartWatchingThread>(msg =>
        {
            IActorRef scraperActor = GetScraperActor(msg);

            scraperActor.Tell(new StartScraping(), Self);
        });
        Receive<StopWatchingThread>(msg =>
        {
            if (!_threadWatcherActors.TryGetValue((msg.Board, msg.ThreadId), out IActorRef? scraperActor))
                return;

            scraperActor.Tell(new StopScraping());
        });
        Receive<ThreadScraperTerminated>(msg =>
        {
            _logger.Info($"Scraper actor terminated: {msg.Board} {msg.ThreadId}");
            
            if (!_threadWatcherActors.ContainsKey((msg.Board, msg.ThreadId)))
                return;

            _threadWatcherActors.Remove((msg.Board, msg.ThreadId));
        });
        Receive<GetThreadScrapingStatus>(msg =>
        {
            if (!_threadWatcherActors.TryGetValue((msg.Board, msg.ThreadId), out var scraperActor))
            {
                Sender.Tell(new ThreadScrapingStatusReturned(msg.Board, msg.ThreadId, false));
                return;
            }
            
            scraperActor.Forward(msg);
        });
    }

    private IActorRef GetScraperActor(StartWatchingThread msg)
    {
        if (!_threadWatcherActors.TryGetValue((msg.Board, msg.ThreadId), out IActorRef? scraperActor))
        {
            scraperActor = Context.ActorOf(DependencyResolver.For(Context.System).Props<ThreadScraperActor>(),
                $"{msg.Board}-{msg.ThreadId}-scraper");
            _threadWatcherActors.Add((msg.Board, msg.ThreadId), scraperActor);

            scraperActor.Tell(new SetTargetThread(msg.Board, msg.ThreadId, _downloadLocation));
        }

        return scraperActor;
    }
}
