using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ChanScraper.Gui.ViewModels;

namespace ChanScraper.Gui.Views;

public partial class ScrapeThreadView : UserControl
{
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
        throw new NotImplementedException();
    }
}
