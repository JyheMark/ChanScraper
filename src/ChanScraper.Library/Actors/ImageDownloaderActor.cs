using Akka.Actor;
using Akka.Event;
using ChanScraper.ChanApi;
using ChanScraper.ChanApi.Extensions;
using ChanScraper.ChanApi.Models;
using ChanScraper.Library.Actors.Messages;

namespace ChanScraper.Library.Actors;

internal sealed class ImageDownloaderActor : ReceiveActor
{
    private readonly IChanClient _client;
    private readonly string _downloadPath;
    private readonly ILoggingAdapter _loggingAdapter;
    private readonly List<int> _processedPosts;

    public ImageDownloaderActor(IChanClient client, string downloadPath)
    {
        _client = client;
        _processedPosts = new List<int>();
        _loggingAdapter = Context.GetLogger();
        _downloadPath = downloadPath;

        Become(Ready);
    }

    private void Ready()
    {
        Receive<DownloadImages>(msg =>
        {
            Become(Downloading);
            DownloadImages(msg.Board, msg.GetThreadResponse);
            Become(Ready);
        });
    }

    private void DownloadImages(string board, GetThreadResponse getThreadResponse)
    {
        string directoryPath = GetDirectoryPath(BuildDirectoryName(board, getThreadResponse));

        foreach (Post post in getThreadResponse.Posts.Where(p => p.HasAttachment() && !_processedPosts.Contains(p.Id.Value)))
        {
            var imageFileName = $"{post.FileName}--{post.Time}{post.FileExtension}";
            _loggingAdapter.Info($"Fetching image {imageFileName}");

            HttpResponseMessage imageResponse = _client.GetImageAsync(board, post).GetAwaiter().GetResult();

            if (!imageResponse.IsSuccessStatusCode)
            {
                _loggingAdapter.Warning($"Failed to retrieve {imageFileName}");
                continue;
            }

            byte[] bytes = imageResponse.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();

            using FileStream write = File.OpenWrite(Path.Join(directoryPath, imageFileName));
            write.Write(bytes);

            _processedPosts.Add(post.Id.Value);
        }
    }

    private string GetDirectoryPath(string directoryName)
    {
        var directoryPath = Path.Join(_downloadPath, directoryName);

        if (!Directory.Exists(directoryName))
            Directory.CreateDirectory(directoryPath);
        return directoryPath;
    }

    private static string BuildDirectoryName(string board, GetThreadResponse getThreadResponse)
    {
        var directoryName = $"{board}--{getThreadResponse.Posts.First().Id}";

        var threadTitle = getThreadResponse.Posts.First().Title;
        
        if (!string.IsNullOrWhiteSpace(threadTitle))
            directoryName += $"--{threadTitle}";
        else
        {
            var threadComment = getThreadResponse.Posts.First().Comment;
            
            if (!string.IsNullOrWhiteSpace(threadComment))
                directoryName += $"--{threadComment}";
        }
        
        var directoryNameCleaned = string.Join("", directoryName.Split(Path.GetInvalidFileNameChars()));

        return directoryNameCleaned
            .Replace(" ", "_");
    }

    private void Downloading()
    {
        ReceiveAny(_ => { });
    }
}
