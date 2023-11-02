namespace BaverGame.Controllers.Parsing.Core.API;

public interface IParserSettings
{
    public string URL { get; set; }
    public string[] PriceElements { get; set; }
}