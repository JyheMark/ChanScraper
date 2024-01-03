namespace ChanScraper.ChanApi;

internal static class BoardTokenResolver
{
    public static string Resolve(Board board)
    {
        return board switch
        {
            Board.Random => "b",
            Board.Hentai => "h",
            _ => throw new ArgumentOutOfRangeException(nameof(board), board, null)
        };
    }
}
