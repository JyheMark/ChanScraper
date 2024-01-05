namespace ChanScraper.ChanApi.Models;

public sealed record GetThreadResponse
{
    public IEnumerable<Post>? Posts { get; init; }
}