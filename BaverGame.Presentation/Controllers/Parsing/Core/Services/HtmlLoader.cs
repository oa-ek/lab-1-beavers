using System.Net;
using BaverGame.Controllers.Parsing.Core.API;

namespace BaverGame.Controllers.Parsing.Core.Services;

internal class HtmlLoader
{
    private readonly HttpClient _client;
    private readonly string _url;

    public HtmlLoader(IParserSettings settings)
    {
        _client = new HttpClient();
        _url = settings.URL;
    }

    public async Task<string?> GetSource()
    {
        var response = await _client.GetAsync(_url);
        string? source = null;
        
        var dataReceived = response is not null && response.StatusCode == HttpStatusCode.OK;
        if (dataReceived)
            source = await response.Content.ReadAsStringAsync();

        return source;
    }
}