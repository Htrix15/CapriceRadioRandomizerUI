using HtmlAgilityPack;
using Services;

namespace Tests;

internal class CapricePageServiceTests
{
    private HtmlDocument _mainPageMock;
    private HtmlNode _genresTableMock;
    private HtmlNode _mainGenreTableMock;

    private async Task InitMainPageMocFromFile()
    {
        using var reader = new StreamReader(@"./TestHtmls/MainPage.html");
        string html = await reader.ReadToEndAsync();
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        _mainPageMock = doc;
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

    [SetUp]
    public async Task Setup()
    {
        await InitMainPageMocFromFile();
        await InitGenreTableMocFromFile();
        await InitMainGenreTableMocFromFile();
    }

    [Test]
    public async Task GetCapricePage_ShouldNotThrow()
    {
        var service = new CapricePageService();

        Assert.DoesNotThrowAsync(async () => await service.GetCapricePage());
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
}
