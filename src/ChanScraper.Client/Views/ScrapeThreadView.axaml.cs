using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace ChanScraper.Client.Views;

public partial class ScrapeThreadView : UserControl
{
    private bool _isScraping;
    
    public ScrapeThreadView()
    {
        InitializeComponent();
    }

    private async void BrowseButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        ArgumentNullException.ThrowIfNull(topLevel);

        IReadOnlyList<IStorageFolder> directory = await topLevel.StorageProvider.OpenFolderPickerAsync(
            new FolderPickerOpenOptions
            {
                Title = "Download to",
                AllowMultiple = false
            });

        if (!directory.Any())
            return;

        DownloadDirectoryPath.Text = directory[0].Path.AbsolutePath; 
    }

    private void ScrapeButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(DownloadDirectoryPath.Text) || string.IsNullOrWhiteSpace(ThreadUrl.Text))
            return;

        if (!Uri.IsWellFormedUriString(ThreadUrl.Text, UriKind.Absolute))
            return;

        if (!_isScraping)
        {
            StartScraping();
            _isScraping = true;
        }
        else
        {
            StopScraping();
            _isScraping = false;
        }
    }

    private void StopScraping()
    {
        ScrapeButton.Content = "Scrape Images";
        BrowseButton.IsEnabled = true;
        ThreadUrl.IsEnabled = true;
    }

    private void StartScraping()
    {
        ScrapeButton.Content = "Stop Scraping";
        BrowseButton.IsEnabled = false;
        ThreadUrl.IsEnabled = false;
    }
}
