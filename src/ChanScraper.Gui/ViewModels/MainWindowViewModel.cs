using System.Collections.Generic;
using Akka.Actor;
using Akka.Hosting;
using ChanScraper.ChanApi.Models;
using ChanScraper.Library.Actors;
using ChanScraper.Library.Actors.Messages;
using ReactiveUI;

namespace ChanScraper.Gui.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string _downloadDirectory = string.Empty;
    private readonly IActorRef _chanActor;
    private readonly List<ScrapeThreadViewModel> _scrapeThreadViewModels;

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

    public void ToggleThreadScraping(Board board, int threadId)
    {
        _chanActor.Tell(new WatchThread(board.ShortTitle, threadId), ActorRefs.NoSender);
    }
}
