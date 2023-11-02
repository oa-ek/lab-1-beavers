using AngleSharp.Html.Dom;

namespace BaverGame.Controllers.Parsing.API;

public interface IParser<T>
{
    public T Parse(IHtmlDocument document);
}