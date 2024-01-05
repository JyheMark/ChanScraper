using ChanScraper.ChanApi.Models;

namespace ChanScraper.ChanApi.Extensions;

public static class PostExtensions
{
    public static bool HasAttachment(this GetThreadResponse.Post post)
    {
        return !string.IsNullOrWhiteSpace(post.FileName) && !string.IsNullOrWhiteSpace(post.FileExtension);
    }
}
