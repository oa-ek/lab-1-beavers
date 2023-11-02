using AngleSharp.Html.Parser;
using BaverGame.Controllers.Parsing.Core.API;

namespace BaverGame.Controllers.Parsing.Core.Services;

public class ParserWorker<T>
{
    public ParserWorker(IParser<T> parser) => 
        Parser = parser;

    public ParserWorker(IParser<T> parser, IParserSettings settings)
    : this(parser) =>
        ParserSettings = settings;

    private IParser<T> _parser;
    private IParserSettings _parserSettings;
    private HtmlLoader _htmlLoader;
    private HtmlParser _domParser = new();
    private TaskCompletionSource<T> _taskCompletionSource;

    public Task<T> Task => _taskCompletionSource.Task;
    public event Action<T> OnCompleted;

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

    public async void Start()
    {
        _taskCompletionSource = new TaskCompletionSource<T>();
        await Parse();
    }
    
    public async Task<T> StartAsync()
    {
        _taskCompletionSource = new TaskCompletionSource<T>();
        return await Parse();
    }

    private async Task<T> Parse()
    {
        var source = await _htmlLoader.GetSource();
        var document = await _domParser.ParseDocumentAsync(source);
        var result = _parser.Parse(document, _parserSettings.PriceElements);

        _taskCompletionSource.SetResult(result);
        OnCompleted?.Invoke(result);
        return result;
    }
}