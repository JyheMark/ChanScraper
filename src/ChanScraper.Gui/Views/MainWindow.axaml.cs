using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ChanScraper.Gui.ViewModels;

namespace ChanScraper.Gui.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
        AppendNewScrapeComponent();
    }

    private void Btn_AddScraper_OnClick(object? sender, RoutedEventArgs e)
    {
        AppendNewScrapeComponent();
    }

    private void AppendNewScrapeComponent()
    {
        var viewModel = DataContext as MainWindowViewModel;
        ArgumentNullException.ThrowIfNull(viewModel);
        
        var scrapeViewModel = viewModel.CreateNewScrapeThreadViewModel();
     
        var scrapeView = new ScrapeThreadView
        {
            DataContext = scrapeViewModel
        };

        pnl_ScrapeThreadAppend.Children.Add(scrapeView);
    }
}
