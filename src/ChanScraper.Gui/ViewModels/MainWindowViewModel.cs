using System.Collections.Generic;
using ReactiveUI;

namespace ChanScraper.Gui.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string _downloadDirectory = string.Empty;

    private readonly List<ScrapeThreadViewModel> _scrapeThreadViewModels;

    public MainWindowViewModel()
    {
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
}
