using AngleSharp.Html.Parser;
using BaverGame.Controllers.Parsing.API;

namespace BaverGame.Controllers.Parsing.Services;

public class ParserWorker<T>
{
    public ParserWorker(IParser<T> parser) => 
        Parser = parser;

    public ParserWorker(IParser<T> parser, IParserSettings settings)
    : this(parser) =>
        ParserSettings = settings;

    public event Action<T> OnCompleted;
    private IParser<T> _parser;
    private IParserSettings _parserSettings;
    private HtmlLoader _htmlLoader;
    private HtmlParser _domParser = new();
    
    public IParser<T> Parser
    {
        get => _parser;
        set => _parser = value;
    }

    public IParserSettings ParserSettings
    {
        get => _parserSettings;
        set
        {
            _parserSettings = value;
            _htmlLoader = new HtmlLoader(_parserSettings);
        }
    }

    public void Start() => Parse();

    private async void Parse()
    {
        var source = await _htmlLoader.GetSource();
        var document = await _domParser.ParseDocumentAsync(source);
        var result = _parser.Parse(document);
        
        OnCompleted?.Invoke(result);
    }
}