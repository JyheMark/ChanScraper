using Avalonia.Controls;
using Avalonia.Interactivity;
using ChanScraper.Gui.ViewModels;

namespace ChanScraper.Gui.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        AppendNewScrapeComponent();
    }

    private void Btn_AddScraper_OnClick(object? sender, RoutedEventArgs e)
    {
        AppendNewScrapeComponent();
    }

    private void AppendNewScrapeComponent()
    {
        var scrapeView = new ScrapeThreadView();
        var scrapeViewModel = new ScrapeThreadViewModel();
        scrapeView.DataContext = scrapeViewModel;
        
        pnl_ScrapeThreadAppend.Children.Add(scrapeView);
    }
}
