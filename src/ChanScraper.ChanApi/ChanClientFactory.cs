namespace ChanScraper.ChanApi;

public static class ChanClientFactory
{
    public static ChanClient Create()
    {
        return new ChanClient(new HttpClient());
    }
}
