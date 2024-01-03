using System.Text.Json.Serialization;

namespace ChanScraper.ChanApi.Models;

public sealed record Thread
{
    [JsonPropertyName("no")] public int? Number { get; init; }
    [JsonPropertyName("sub")] public string? Title { get; init; }
    [JsonPropertyName("com")] public string? Comment { get; init; }
    public int? Replies { get; init; }
    public int? Images { get; init; }
}
