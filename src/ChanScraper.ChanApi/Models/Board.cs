using System.Text.Json.Serialization;

namespace ChanScraper.ChanApi.Models;

public sealed record Board
{
    [JsonPropertyName("board")] public string? ShortTitle { get; init; }

    public string? Title { get; init; }

    [JsonPropertyName("meta_description")] public string? Description { get; init; }
}
