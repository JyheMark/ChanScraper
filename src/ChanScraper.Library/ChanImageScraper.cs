using System.Net.Http.Json;
using ChanScraper.ChanApi;
using ChanScraper.ChanApi.Models;
using Thread = System.Threading.Thread;

namespace ChanScraper.Library;

public class ChanImageScraper
{
    private readonly IChanClient _chanClient;
    private readonly List<int> _processedPosts;
    private CancellationTokenSource? _cancellationTokenSource;

    public ChanImageScraper(IChanClient chanClient)
    {
        _chanClient = chanClient;
        _processedPosts = new List<int>();
    }

    public void Cancel()
    {
        if (_cancellationTokenSource is { IsCancellationRequested: false })
            _cancellationTokenSource.Cancel();
    }

    public async Task BeginAsync(Board board, int postNumber, string downloadLocation)
    {
        if (_cancellationTokenSource is { IsCancellationRequested: false })
            throw new InvalidOperationException(); 
        
        _cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = _cancellationTokenSource.Token;
        
        await Task.Run(async () =>
        {
            Thread.Sleep(5000);
            while (!cancellationToken.IsCancellationRequested)
            {
                var response = await _chanClient.GetPostAsync(board, postNumber);
                
                if (!response.IsSuccessStatusCode)
                    continue;

                var posts = await response.Content.ReadFromJsonAsync<IEnumerable<Post>>(cancellationToken: cancellationToken);
                
                if (posts == null || !posts.Any())
                    continue;

                await ProcessPosts(board, posts, downloadLocation);
            }
        }, cancellationToken);
    }

    private async Task ProcessPosts(Board board, IEnumerable<Post> posts, string downloadLocation)
    {
        foreach (var post in posts.Where(p => !string.IsNullOrWhiteSpace(p.FileName) && !_processedPosts.Contains(p.Number)))
        {
            var request = await _chanClient.GetImageAsync(board, post);
            
            if (!request.IsSuccessStatusCode)
                continue;

            var imageStream = await request.Content.ReadAsByteArrayAsync();

            await using var writer = File.OpenWrite($"{downloadLocation}/{post.Time}.{post.FileExtension}");
            await writer.WriteAsync(imageStream);
            
            _processedPosts.Add(post.Number);
        }
    }
}
