namespace ChanScraper.ChanApi.Models;

public sealed record GetBoardsResponse
{
    public IEnumerable<Board>? Boards { get; init; }
}