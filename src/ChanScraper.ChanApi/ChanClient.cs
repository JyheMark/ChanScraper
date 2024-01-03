using System.Globalization;
using ChanScraper.ChanApi.Models;

namespace ChanScraper.ChanApi;

public interface IChanClient
{
    Task<HttpResponseMessage> GetCatalogAsync(Board board);
    Task<HttpResponseMessage> GetPostAsync(Board board, int postNumber);
    Task<HttpResponseMessage> GetImageAsync(Board board, Post post);
}

public sealed class ChanClient : IChanClient
{
    private readonly HttpClient _client;

    public ChanClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<HttpResponseMessage> GetCatalogAsync(Board board)
    {
        const string uriTemplate = "https://a.4cdn.org/{board}/catalog.json";
        string boardToken = ResolveBoardUriToken(board);
        
        var request = new HttpRequestMessage(HttpMethod.Get, uriTemplate.Replace("{board}", boardToken));
        return await _client.SendAsync(request);
    }

    public async Task<HttpResponseMessage> GetPostAsync(Board board, int postNumber)
    {
        const string uriTemplate = "https://a.4cdn.org/{board}/thread/{postNumber}.json";
        var boardToken = ResolveBoardUriToken(board);

        var uri = uriTemplate
            .Replace("{board}", boardToken)
            .Replace("{postNumber}", postNumber.ToString());
        
        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        return await _client.SendAsync(request);
    }

    public async Task<HttpResponseMessage> GetImageAsync(Board board, Post post)
    {
        const string uriTemplate = "https://i.4cdn.org/{board}/{postTime}.{extension}";
        
        if (string.IsNullOrWhiteSpace(post.FileName) || string.IsNullOrWhiteSpace(post.FileExtension))
            throw new InvalidOperationException();

        var boardToken = ResolveBoardUriToken(board);

        var uri = uriTemplate
            .Replace("{board}", boardToken)
            .Replace("{postTime}", post.Time.ToString(CultureInfo.InvariantCulture))
            .Replace("{extension}", post.FileExtension);

        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        return await _client.SendAsync(request);
    }

    private string ResolveBoardUriToken(Board board)
    {
        return BoardTokenResolver.Resolve(board);
    }
}
