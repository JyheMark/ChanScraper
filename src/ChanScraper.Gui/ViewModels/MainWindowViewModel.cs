namespace ChanScraper.Gui.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        ScrapeThread = new ScrapeThreadViewModel();
    }
    
    public ScrapeThreadViewModel ScrapeThread { get; set; }
}
