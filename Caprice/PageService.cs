using HtmlAgilityPack;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Caprice;

public partial class PageService: IPageService, IRemoteService
{
    [GeneratedRegex(@"file:\[(\S*)]}\);")]
    private static partial Regex SearchPlayerJsParams();

    [GeneratedRegex(@"\{(?:[^{}]|(?<DEPTH>\{)|(?<-DEPTH>\}))+(?(DEPTH)(?!))\}", RegexOptions.Singleline)]
    private static partial Regex SearchJsonObjects();

    [GeneratedRegex(@"\s+")]
    private static partial Regex NormalizedText();

    public async Task<HtmlDocument> GetPage(string url)
    {
        var page = await new HtmlWeb().LoadFromWebAsync(url);
        return page ?? throw new Exception("MainPage not found!");
    }

    public HtmlNodeCollection SearchGenresTables(HtmlDocument mainPage)
    {
        return mainPage.DocumentNode.SelectNodes($"//table[contains(@id, '{Constants.GenreTableIdSubStr}')]") ?? throw new Exception("Genres tables not found!");
    }

    public HtmlNode SearchMainGenreTable(HtmlNode genreTable)
    {
        var previousSibling = genreTable.PreviousSibling.PreviousSibling;
        return previousSibling.Name.Equals("table", StringComparison.OrdinalIgnoreCase) ? previousSibling : throw new Exception("Previous tables not found!");
    }

    public string GetMainGenreName(HtmlNode mainGenreTable)
    {
        return mainGenreTable.InnerText.Trim();
    }

    public string GetMainGenreKey(HtmlNode mainGenreTable)
    {
        var linkNode = mainGenreTable.SelectSingleNode(".//a[@href]")
            ?? throw new Exception("Genre link not found!");

        var href = linkNode.GetAttributeValue("href", string.Empty);

        return href.Split("/").Last().Replace(".html", "");
    }

    public List<(string genreName, string url)> GetSubGenresLinks(HtmlNode genreTable)
    {
        var linksNodes = genreTable.SelectNodes(".//a[@href]") ?? throw new Exception("Genres links not found!");
   
        var result = new List<(string genreName, string url)>();

        foreach (var linkNode in linksNodes)
        {
            var genreName = linkNode.FirstChild.InnerText.Trim();
            var url = linkNode.GetAttributeValue("href", string.Empty);
            result.Add((genreName, url));
        }

        return result;
    }

    public string CreateSubGenreLink(string link)
    {
        return link.StartsWith(Constants.RootPage, StringComparison.CurrentCultureIgnoreCase)
            ? link
            : $"{Constants.RootPage}/{link}";
     }

    public HtmlNode SearchPlayerJsScript(HtmlDocument page)
    {
        var table = page.DocumentNode.SelectSingleNode("//table[contains(@class, 'player-big')]") ?? throw new Exception("Table with player not found!");
        return table.SelectSingleNode(".//script") ?? throw new Exception("Player script not found!");
    }     

    public string GetPlayerJsScriptParamsStr(HtmlNode node)
    {
        var normalizedText = NormalizedText().Replace(node.InnerHtml, "");
        var match = SearchPlayerJsParams().Match(normalizedText);
        if (!match.Success) throw new Exception("Player script params not parsed!");
        var jsParams = match.Groups[1].Value;
        return match.Groups[1].Value;
    }

    public List<string> SplitePlayerJsScriptParams(string str)
    {
        var regex = SearchJsonObjects();
        var matches = regex.Matches(str);
        if (!matches.All(m => m.Success)) throw new Exception("Not all script params swgmwnts valid!");
        return [.. matches.Select(m => m.Value.Replace(",file:", ",\"file\":"))];
    }

    public List<(string title, string file)> ExtractPlayerJsScriptParamsValues(List<string> jsons)
    {
        List<(string title, string file)> result = [];

        foreach(var json in jsons)
        {
            var item = JsonSerializer.Deserialize<PlayerJsParams>(json, options: new () { PropertyNameCaseInsensitive = true });
            result.Add((item!.Title, item!.File));
        }

        return result;
    }

    private class PlayerJsParams
    {
        public required string Title { get; set; }
        public required string File { get; set; }
    }

    public string GetGenreKey(List<(string title, string file)> jsParams)
    {
        return jsParams.First().file.Split("/").Last();
    }

    private string CheckAndAddProtocol(string link, string protocol = "http") => link.StartsWith("//") ? $"{protocol}:{link}" : link;

    public RemoteSources CreateRemoteSourcesFromJsParams(List<(string title, string file)> jsParams)
    {
        var trackInfoBaseLink = jsParams.First(p => p.title == "2").file;
        var key = GetGenreKey(jsParams);
        trackInfoBaseLink = CheckAndAddProtocol(trackInfoBaseLink.Replace(key, $"status.xsl?mount=/{key}"));

        return new RemoteSources() { 
            Key = key,
            PlayLink = CheckAndAddProtocol(jsParams.First(p => p.title == "1").file),
            TrackInfoBaseLink = trackInfoBaseLink,
        };
    }

    public async Task<List<Genre>> CreateGenres() => await CreateGenres(await GetPage(Constants.MainPagehUrl));

    public async Task<List<Genre>> CreateGenres(HtmlDocument mainPage)
    {
        List<Genre> result = [];

        var genresTables = SearchGenresTables(mainPage);

        foreach(var genreTable in genresTables)
        {
            var mainGenreTable = SearchMainGenreTable(genreTable);
            var mainGenreName = GetMainGenreName(mainGenreTable);
            var mainGenreKey = GetMainGenreKey(mainGenreTable);
            var subGenresLinks = GetSubGenresLinks(genreTable);

            foreach(var subGenresLink in subGenresLinks)
            {
                var subGenreLink = CreateSubGenreLink(subGenresLink.url);
                var genrePage = await GetPage(subGenreLink);
                var playerJsScript = SearchPlayerJsScript(genrePage);
                var jsScriptParamsStr = GetPlayerJsScriptParamsStr(playerJsScript);
                var jsScriptParams = SplitePlayerJsScriptParams(jsScriptParamsStr);
                var jsScriptParamsValues = ExtractPlayerJsScriptParamsValues(jsScriptParams);
                var genreKey = GetGenreKey(jsScriptParamsValues);
                var remoteSources = CreateRemoteSourcesFromJsParams(jsScriptParamsValues);
                remoteSources.Key = genreKey;

                result.Add(new Genre()
                {
                    Key = genreKey,
                    Name = subGenresLink.genreName,
                    ItIsParent = false,
                    ParentGenreKey = mainGenreKey,
                    ParentGenre = new Genre()
                    {
                        Key = mainGenreKey,
                        Name = mainGenreName,
                        ItIsParent = true,
                        IsAvailable = true,
                        IsDisabled = false,
                        IsSkip = false,
                        TrackCount = 0,
                        RatingCount = 0,
                        Rating = 0,

                    },
                    IsAvailable = true,
                    IsDisabled = false,
                    IsSkip = false,
                    TrackCount = 0,
                    RatingCount = 0,
                    Rating = 0,
                    RemoteSources = remoteSources,
                });
            }
        }

        return result;
    }

    public string GetInnerTextByPath(HtmlDocument page, string path)
    {
        var node = page.DocumentNode.SelectSingleNode(path) ?? throw new Exception($"Node by path {path} not found");
        return node.InnerText;
    }
}
