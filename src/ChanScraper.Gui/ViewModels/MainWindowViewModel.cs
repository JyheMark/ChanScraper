using System.Collections.Generic;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Hosting;
using ChanScraper.ChanApi.Models;
using ChanScraper.Library.Actors;
using ChanScraper.Library.Actors.Messages;
using ReactiveUI;
using Path = System.IO.Path;

namespace ChanScraper.Gui.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IActorRef _chanActor;
    private readonly List<ScrapeThreadViewModel> _scrapeThreadViewModels;
    private string _downloadDirectory = string.Empty;

    public MainWindowViewModel()
    {
        _chanActor = App.GetService<IRequiredActor<ChanScraperEntryActor>>().ActorRef;
        _scrapeThreadViewModels = new List<ScrapeThreadViewModel>();
    }

    public string DownloadDirectory
    {
        get => _downloadDirectory;
        set => this.RaiseAndSetIfChanged(ref _downloadDirectory, value);
    }

    public ScrapeThreadViewModel CreateNewScrapeThreadViewModel()
    {
        var scrapeThreadViewModel = new ScrapeThreadViewModel();
        _scrapeThreadViewModels.Add(scrapeThreadViewModel);
        return scrapeThreadViewModel;
    }

    public void SetDownloadDirectory(string downloadDirectory)
    {
        _chanActor.Tell(new SetDownloadLocation(downloadDirectory), ActorRefs.NoSender);
    }

    public async Task<bool> ToggleThreadScraping(Board board, int threadId)
    {
        if (string.IsNullOrWhiteSpace(_downloadDirectory) || !Path.Exists(_downloadDirectory))
            return false;

        if (!await IsWatchingThread(board, threadId))
        {
            StartWatchingThread(board, threadId);
            return true;
        }
        
        StopWatchingThread(board, threadId);
        return false;
    }

    private async Task<bool> IsWatchingThread(Board board, int threadId)
    {
        var status = await _chanActor.Ask(new GetThreadScrapingStatus(board.ShortTitle, threadId)) as ThreadScrapingStatusReturned;
        return status.IsScraping;
    }

    private void StartWatchingThread(Board board, int threadId)
    {
        _chanActor.Tell(new StartWatchingThread(board.ShortTitle, threadId), ActorRefs.NoSender);
    }

    private void StopWatchingThread(Board board, int threadId)
    {
        _chanActor.Tell(new StopWatchingThread(board.ShortTitle, threadId), ActorRefs.NoSender);
    }
}
