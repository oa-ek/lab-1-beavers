using BaverGame.Controllers.Parsing.Core.API;

namespace BaverGame.Controllers.Parsing;

internal sealed class PriceParserSettings : IParserSettings
{
    public string URL { get; set; }
    public string[] PriceElements { get; set; }
    
    public PriceParserSettings(string url, string[] priceElements)
    {
        URL = url;
        PriceElements = priceElements;
    }
}