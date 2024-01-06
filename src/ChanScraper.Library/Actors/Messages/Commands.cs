using ChanScraper.ChanApi.Models;

namespace ChanScraper.Library.Actors.Messages;

public sealed record SetDownloadLocation(string Path);

public sealed record StartWatchingThread(string Board, int ThreadId);
public sealed record StopWatchingThread(string Board, int ThreadId);

public sealed record GetThreadScrapingStatus(string Board, int ThreadId);

internal sealed record StartScraping;

internal sealed record StopScraping;

internal sealed record DownloadImages(string Board, GetThreadResponse GetThreadResponse);

internal sealed record SetTargetThread(string Board, int ThreadId, string DownloadPath);
