using System.Text.Json.Serialization;

namespace ChanScraper.ChanApi.Models;

public sealed record Page
{
    [JsonPropertyName("page")] 
    public int PageNumber { get; init; }

    public IEnumerable<Thread>? Threads { get; init; }
}
