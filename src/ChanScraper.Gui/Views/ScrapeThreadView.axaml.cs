using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ChanScraper.Gui.ViewModels;
using ChanScraper.Gui.Views.Events;

namespace ChanScraper.Gui.Views;

public partial class ScrapeThreadView : UserControl
{
    public event EventHandler<ScrapeToggleButtonPressed> ButtonClicked;
    private ScrapeThreadViewModel ViewModel => DataContext as ScrapeThreadViewModel;
    
    public ScrapeThreadView()
    {
        InitializeComponent();
        DataContext = new ScrapeThreadViewModel();
        InitializeBoardSelectorComboBox();
    }

    private async void InitializeBoardSelectorComboBox()
    {
        cbx_BoardSelector.ItemsSource = await ViewModel.GetBoardsAsync();
        cbx_BoardSelector.SelectedIndex = 0;
    }

    private void Btn_ToggleStart_OnClick(object? sender, RoutedEventArgs e)
    {
        if (!int.TryParse(ViewModel.ThreadIdInput, out var threadId))
            return;
        
        var selectedBoard = ViewModel.GetBoardByIndex(cbx_BoardSelector.SelectedIndex);
        
        ButtonClicked.Invoke(sender, new ScrapeToggleButtonPressed(selectedBoard, threadId, this));
    }

    public void SetScraping(bool isScraping)
    {
        if (isScraping)
            SetIsScraping();
        else SetNotScraping();
    }

    private void SetIsScraping()
    {
        cbx_BoardSelector.IsEnabled = false;
        txt_ThreadIdInput.IsEnabled = false;

        btn_ToggleStart.Content = "Stop";
        txt_ScraperStatus.Text = "Scraping...";
    }

    private void SetNotScraping()
    {
        btn_ToggleStart.Content = "Start";
        txt_ScraperStatus.Text = "Stopped";
    }
}
