namespace ChanScraper.Library.Actors.Messages;

public sealed record ThreadScrapingStatusReturned(string Board, int ThreadId, bool IsScraping);
internal sealed record ThreadScraperTerminated(string Board, int ThreadId);
