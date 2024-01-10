using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ChanScraper.ChanApi;
using ChanScraper.ChanApi.Models;
using ReactiveUI;

namespace ChanScraper.Gui.ViewModels;

public sealed class ScrapeThreadViewModel : ViewModelBase
{
    private static IEnumerable<Board>? _boards;
    private string _threadIdInput = string.Empty;

    public string ThreadIdInput
    {
        get => _threadIdInput;
        set => this.RaiseAndSetIfChanged(ref _threadIdInput, value);
    }

    public Board GetBoardByIndex(int index)
    {
        return _boards.ElementAt(index);
    }

    public async Task<IEnumerable<Board>> GetBoardsAsync()
    {
        if (_boards != null && _boards.Any())
            return _boards;

        var chanClient = ServiceLocator.GetService<IChanClient>();
        HttpResponseMessage response = await chanClient.GetBoardsAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception("Could not retrieve boards");

        GetBoardsResponse? result = response.Content
            .ReadFromJsonAsync<GetBoardsResponse>()
            .GetAwaiter()
            .GetResult();
        
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(result.Boards);

        _boards = result.Boards;
        return _boards;
    }
}
