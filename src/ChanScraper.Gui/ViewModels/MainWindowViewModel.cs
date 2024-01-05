using ReactiveUI;

namespace ChanScraper.Gui.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string _downloadDirectory = string.Empty;

    public MainWindowViewModel()
    {
        ScrapeThread = new ScrapeThreadViewModel();
    }

    public ScrapeThreadViewModel ScrapeThread { get; set; }

    public string DownloadDirectory
    {
        get => _downloadDirectory;
        set => this.RaiseAndSetIfChanged(ref _downloadDirectory, value);
    }
}
