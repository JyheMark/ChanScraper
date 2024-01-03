namespace ChanScraper.ChanApi.Models;

public record Thread
{
    public IEnumerable<Post>? Posts { get; init; }
}
