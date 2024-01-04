using System.Text.Json.Serialization;

namespace ChanScraper.ChanApi.Models;

public sealed record Post
{
    [JsonPropertyName("no")] public int? Id { get; init; }
    [JsonPropertyName("com")] public string? Comment { get; init; }
    [JsonPropertyName("sub")] public string? Title { get; init; }
    public string? FileName { get; init; }
    [JsonPropertyName("ext")] public string? FileExtension { get; init; }
    [JsonPropertyName("tim")] public long? Time { get; init; }
}
