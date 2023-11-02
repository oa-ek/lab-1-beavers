using AngleSharp.Html.Dom;

namespace BaverGame.Controllers.Parsing.Core.API;

public interface IParser<T>
{
    public T Parse(IHtmlDocument document, string[] priceElements);
}