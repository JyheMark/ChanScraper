using System.Net.Http.Json;
using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Event;
using ChanScraper.ChanApi;
using ChanScraper.ChanApi.Models;
using ChanScraper.Library.Actors.Messages;

namespace ChanScraper.Library.Actors;

internal sealed class ThreadScraperActor : ReceiveActor
{
    private static readonly TimeSpan _scrapeInterval = TimeSpan.FromSeconds(20);
    private readonly IChanClient _chanClient;
    private readonly ILoggingAdapter _logger;
    private string? _board;
    private IActorRef _downloaderActor;
    private string? _downloadPath;
    private int? _threadId;

    public ThreadScraperActor(IChanClient chanClient)
    {
        _chanClient = chanClient;
        _logger = Context.GetLogger();

        _logger.Info("Starting");

        Receive<StartScraping>(msg =>
        {
            _board = msg.Board;
            _threadId = msg.ThreadId;
            _downloadPath = msg.DownloadPath;
            _downloaderActor = Context.ActorOf(DependencyResolver.For(Context.System).Props<ImageDownloaderActor>(_downloadPath),
                $"{_board}-{_threadId}-downloader");

            Become(Scraping);
            Self.Tell(new Scrape(), Self);
        });
    }

    private void Scraping()
    {
        Receive<Scrape>(_ =>
        {
            PerformScrape();
            ScheduleNext();
        });
    }

    private void PerformScrape()
    {
        if (string.IsNullOrWhiteSpace(_board) || !_threadId.HasValue)
        {
            _logger.Error("Bad thread / board combination.");
            Terminate();
            return;
        }

        _logger.Info($"Checking thread {_threadId} on /{_board}/");

        HttpResponseMessage threadResponse =
            _chanClient.GetPostsAsync(_board, _threadId.Value).GetAwaiter().GetResult();

        if (!threadResponse.IsSuccessStatusCode)
        {
            _logger.Warning("Failed to fetch thread. Thread may be expired.");
            Terminate();
            return;
        }

        GetThreadResponse? thread = threadResponse.Content.ReadFromJsonAsync<GetThreadResponse>().GetAwaiter().GetResult();

        if (thread == null)
        {
            _logger.Error("Could not deserialise thread.");
            Terminate();
            return;
        }

        _downloaderActor.Tell(new DownloadImages(_board, thread), Self);
    }

    private void Terminate()
    {
        _logger.Info("Terminating.");
        Context.Stop(Self);
    }

    private void ScheduleNext()
    {
        Context.System.Scheduler.ScheduleTellOnce(_scrapeInterval, Self, new Scrape(), Self);
    }

    private record Scrape;
}
