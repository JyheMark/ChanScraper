using ReactiveUI;

namespace ChanScraper.Gui.ViewModels;

public sealed class ScrapeThreadViewModel : ViewModelBase
{
    private string _threadIdInput = string.Empty;

    public string ThreadIdInput
    {
        get => _threadIdInput;
        set => this.RaiseAndSetIfChanged(ref _threadIdInput, value);
    }
}
