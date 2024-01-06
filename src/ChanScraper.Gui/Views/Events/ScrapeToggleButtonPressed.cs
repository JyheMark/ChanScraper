using ChanScraper.ChanApi.Models;

namespace ChanScraper.Gui.Views.Events;

public sealed record ScrapeToggleButtonPressed(
    Board Board,
    int ThreadId,
    ScrapeThreadView Sender
);
