using ReactiveUI;

namespace ChanScraper.Client.ViewModels;

public sealed class ScrapeThreadViewModel : ViewModelBase
{
    private string _downloadDirectory = string.Empty;
    private string _threadUri = string.Empty;

    public string DownloadDirectory
    {
        get => _downloadDirectory;
        set => this.RaiseAndSetIfChanged(ref _downloadDirectory, value);
    }

    public string ThreadUri
    {
        get => _threadUri;
        set => this.RaiseAndSetIfChanged(ref _threadUri, value);
    }
}
