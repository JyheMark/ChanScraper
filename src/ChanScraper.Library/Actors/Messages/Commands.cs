using Thread = ChanScraper.ChanApi.Models.Thread;

namespace ChanScraper.Library.Actors.Messages;

public sealed record SetDownloadLocation(string Path);

public sealed record WatchThread(string Board, int ThreadId);

internal sealed record StartScraping(string Board, int ThreadId, string DownloadPath);

internal sealed record DownloadImages(string Board, Thread Thread);
