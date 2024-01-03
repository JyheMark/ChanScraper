using System.Text.Json.Serialization;

namespace ChanScraper.ChanApi.Models;

public sealed record Post
{
    [JsonPropertyName("no")] public int? Id { get; set; }
    [JsonPropertyName("com")] public string? Comment { get; set; }
    public string? FileName { get; set; }
    [JsonPropertyName("ext")] public string? FileExtension { get; set; }
    [JsonPropertyName("tim")] public long? Time { get; set; }
}
