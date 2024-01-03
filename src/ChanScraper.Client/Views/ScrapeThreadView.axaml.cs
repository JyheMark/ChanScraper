using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace ChanScraper.Client.Views;

public partial class ScrapeThreadView : UserControl
{
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
}
