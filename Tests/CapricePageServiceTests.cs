using HtmlAgilityPack;
using Infrastructure.Constants;
using Infrastructure.Models;
using Services;

namespace Tests;

internal class CapricePageServiceTests
{
    private HtmlDocument _mainPageMock;
    private HtmlDocument _genrePageMock;
    private HtmlNode _genresTableMock;
    private HtmlNode _mainGenreTableMock;
    private HtmlNode _playerScriptMock;
    private readonly string _jsPlayerParamsStrMoc = @"{""title"":""1"",file:""//79.111.14.76:8002/indianfolk""},{""title"":""2"",file:""//79.111.14.76:8000/indianfolk""},{""title"":""3"",file:""//79.111.14.76:8004/indianfolk""}";
    private readonly List<string> _jsPlayerParamsJsonsMoc = [
        "{\"title\":\"1\",\"file\":\"//79.111.14.76:8002/indianfolk\"}",
        "{\"title\":\"2\",\"file\":\"//79.111.14.76:8000/indianfolk\"}",
        "{\"title\":\"3\",\"file\":\"//79.111.14.76:8004/indianfolk\"}"
        ];

    private readonly List<(string title, string file)> _jsPlayerParamsMoc = [
        ("1", "//79.111.14.76:8002/indianfolk"),
        ("2", "//79.111.14.76:8000/indianfolk"),
        ("3", "//79.111.14.76:8004/indianfolk")
        ];

    private async Task InitMainPageMocFromFile()
    {
        using var reader = new StreamReader(@"./TestHtmls/MainPage.html");
        string html = await reader.ReadToEndAsync();
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        _mainPageMock = doc;
    }

    private async Task InitGenrePageMocFromFile()
    {
        using var reader = new StreamReader(@"./TestHtmls/GenrePage.html");
        string html = await reader.ReadToEndAsync();
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        _genrePageMock = doc;
    }

    private async Task InitGenreTableMocFromFile()
    {
        using var reader = new StreamReader(@"./TestHtmls/GenresTable.html");
        string html = await reader.ReadToEndAsync();
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        _genresTableMock = doc.DocumentNode.SelectNodes($"//table[contains(@id, 'table12')]")[0];
    }

    private async Task InitMainGenreTableMocFromFile()
    {
        using var reader = new StreamReader(@"./TestHtmls/MainGenreTable.html");
        string html = await reader.ReadToEndAsync();
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        _mainGenreTableMock = doc.DocumentNode.FirstChild;
    }

    private async Task InitPlayerScriptMock()
    {
        using var reader = new StreamReader(@"./TestHtmls/PlayerScript.html");
        string html = await reader.ReadToEndAsync();
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        _playerScriptMock = doc.DocumentNode.FirstChild;
    }

    [SetUp]
    public async Task Setup()
    {
        await InitMainPageMocFromFile();
        await InitGenreTableMocFromFile();
        await InitMainGenreTableMocFromFile();
        await InitGenrePageMocFromFile();
        await InitPlayerScriptMock();
    }

    [Test]
    public async Task GetPage_ShouldNotThrow()
    {
        var service = new CapricePageService();

        Assert.DoesNotThrowAsync(async () => await service.GetPage(CapricePageConstants.MainPagehUrl));
    }

    [Test]
    public async Task SearchGenresTables_ShouldNotThrow()
    {
        var service = new CapricePageService();

        Assert.DoesNotThrow(() => service.SearchGenresTables(_mainPageMock));
    }

    [Test]
    public void SearchGenresTables_ShouldReturn3Table()
    {
        var service = new CapricePageService();

        var result = service.SearchGenresTables(_mainPageMock);

        Assert.That(result, Has.Count.EqualTo(3));
    }

    [Test]
    public void SearchMainGenreTable_ShouldNotThrow()
    {
        var service = new CapricePageService();

        Assert.DoesNotThrow(() => service.SearchMainGenreTable(_genresTableMock));
    }

    [Test]
    public void GetMainGenreTableName_ShouldEqualeStr()
    {
        var service = new CapricePageService();

        var mainGenreName = service.GetMainGenreName(_mainGenreTableMock);

        Assert.That(mainGenreName, Is.EqualTo("ETHNIC / FOLK / SPIRITUAL MUSIC"));
    }

    [Test]
    public void GetMainGenreKey_ShouldEqualeStr()
    {
        var service = new CapricePageService();

        var mainGenreKey = service.GetMainGenreKey(_mainGenreTableMock);

        Assert.That(mainGenreKey, Is.EqualTo("ethnic-d"));
    }

    [Test]
    public void GetSubGenresLinks_ShouldNotThrow()
    {
        var service = new CapricePageService();

        Assert.DoesNotThrow(() => service.GetSubGenresLinks(_genresTableMock));
    }

    [Test]
    public void GetSubGenresLinks_ShouldReturnExpectedResult()
    {
        var service = new CapricePageService();

        List<(string genreName, string url)> expected = [
            ("INDIAN CLASSICAL/FOLK/ETHNIC", "http://radcap.ru/indian.html"),
            ("RUSSIAN FOLK", "http://radcap.ru/russian.html"),
            ("CELTIC", "http://radcap.ru/celtic.html")
        ];

        var actual = service.GetSubGenresLinks(_genresTableMock);

        Assert.That(actual, Is.EqualTo(expected).AsCollection);
    }

    [Test]
    public void SearchPlayerJsScript_ShouldNotThrow()
    {
        var service = new CapricePageService();

        Assert.DoesNotThrow(() => service.SearchPlayerJsScript(_genrePageMock));
    }

    [Test]
    public void GetTitleFilePairFromPlayerJsScript_ShouldNotThrow()
    {
        var service = new CapricePageService();

        Assert.DoesNotThrow(() => service.GetPlayerJsScriptParamsStr(_playerScriptMock));
    }

    [Test]
    public void GetTitleFilePairFromPlayerJsScript_ShouldReturnExpectedResult()
    {
        var service = new CapricePageService();

        var expected = _jsPlayerParamsStrMoc;
        var actual = service.GetPlayerJsScriptParamsStr(_playerScriptMock);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void SplitePlayerJsScriptParams_ShouldNotThrow()
    {
        var service = new CapricePageService();

        Assert.DoesNotThrow(() => service.SplitePlayerJsScriptParams(_jsPlayerParamsStrMoc));
    }

    [Test]
    public void SplitePlayerJsScriptParams_ShouldReturnExpectedResult()
    {
        var service = new CapricePageService();

        var expected = _jsPlayerParamsJsonsMoc;

        var actual = service.SplitePlayerJsScriptParams(_jsPlayerParamsStrMoc);

        Assert.That(actual, Is.EqualTo(expected).AsCollection);
    }

    [Test]
    public void ExtractPlayerJsScriptParamsValues_ShouldReturnExpectedResult()
    {
        var service = new CapricePageService();

        var expected = _jsPlayerParamsMoc;

        var actual = service.ExtractPlayerJsScriptParamsValues(_jsPlayerParamsJsonsMoc);

        Assert.That(actual, Is.EqualTo(expected).AsCollection);
    }


    [Test]
    public void GetGenreKey_ShouldReturnExpectedResult()
    {
        var service = new CapricePageService();

        var expected = "indianfolk";

        var actual = service.GetGenreKey(_jsPlayerParamsMoc);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSubGenreLink_ShouldReturnExpectedResult()
    {
        var service = new CapricePageService();

        var expected = "http://radcap.ru/indianfolk";

        var actual = service.CreateSubGenreLink("indianfolk");

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSubGenreLink_WithoutReplace_ShouldReturnExpectedResult()
    {
        var service = new CapricePageService();

        var expected = "http://radcap.ru/indianfolk";

        var actual = service.CreateSubGenreLink("http://radcap.ru/indianfolk");

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void CreateRemoteSourcesFromJsParams_ShouldReturnExpectedResult()
    {
        var service = new CapricePageService();

        var expected = new RemoteSources() { 
            PlayLink = "//79.111.14.76:8002/indianfolk" , 
            TrackInfoBaseLink = "//79.111.14.76:8000/status.xsl?mount=/indianfolk" 
        };

        var actual = service.CreateRemoteSourcesFromJsParams(_jsPlayerParamsMoc);

        Assert.Multiple(() =>
        {
            Assert.That(actual.PlayLink, Is.EqualTo(expected.PlayLink));
            Assert.That(actual.TrackInfoBaseLink, Is.EqualTo(expected.TrackInfoBaseLink));
        });
    }


    [Test]
    public async Task CreateGenres_ShouldReturn10Genre()
    {
        var service = new CapricePageService();

        var result = await service.CreateGenres(_mainPageMock);

        Assert.That(result, Has.Count.EqualTo(10));
    }

    [Test]
    public async Task CreateGenres_ShouldNotThrow()
    {
        var service = new CapricePageService();

        Assert.DoesNotThrowAsync(async () => await service.CreateGenres());
    }
}
