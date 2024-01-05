using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ChanScraper.Gui.ViewModels;

namespace ChanScraper.Gui.Views;

public partial class MainWindow : Window
{
    private MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;

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
        ScrapeThreadViewModel scrapeViewModel = ViewModel.CreateNewScrapeThreadViewModel();

        var scrapeView = new ScrapeThreadView
        {
            DataContext = scrapeViewModel
        };

        scrapeView.ButtonClicked += (_, args) =>
        {
            ViewModel.ToggleThreadScraping(args.Board, args.ThreadId);
        };

        pnl_ScrapeThreadAppend.Children.Add(scrapeView);
    }

    private async void Btn_BrowseDownloadLocation_OnClick(object? sender, RoutedEventArgs e)
    {
        IReadOnlyList<IStorageFolder> selectedDirectory = await GetTopLevel(this)!
            .StorageProvider
            .OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Download location"
            });

        if (!selectedDirectory.Any())
            return;

        var downloadPath = selectedDirectory[0].Path.AbsolutePath;
        
        ViewModel.DownloadDirectory = downloadPath;
        ViewModel.SetDownloadDirectory(downloadPath);
    }
}
