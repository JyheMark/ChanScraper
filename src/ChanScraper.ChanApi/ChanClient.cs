using ChanScraper.ChanApi.Models;

namespace ChanScraper.ChanApi;

public interface IChanClient
{
    Task<HttpResponseMessage> GetPostsAsync(string board, int threadId);
    Task<HttpResponseMessage> GetImageAsync(string board, GetThreadResponse.Post post);
    Task<HttpResponseMessage> GetBoardsAsync();
}

public class ChanClient : IChanClient
{
    private readonly HttpClient _httpClient;

    public ChanClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> GetPostsAsync(string board, int threadId)
    {
        var uri = $"https://a.4cdn.org/{board}/thread/{threadId}.json";

        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        return await _httpClient.SendAsync(request);
    }

    public async Task<HttpResponseMessage> GetImageAsync(string board, GetThreadResponse.Post post)
    {
        var uri = $"https://i.4cdn.org/{board}/{post.Time}{post.FileExtension}";

        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        return await _httpClient.SendAsync(request);
    }

    public async Task<HttpResponseMessage> GetBoardsAsync()
    {
        const string uri = "https://a.4cdn.org/boards.json";

        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        return await _httpClient.SendAsync(request);
    }
}
