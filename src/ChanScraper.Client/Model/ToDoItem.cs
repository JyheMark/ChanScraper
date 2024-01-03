namespace ChanScraper.Client.DataModel;

internal sealed class ToDoItem
{
    public string Description { get; set; } = string.Empty;
    public bool IsChecked { get; set; }
}
