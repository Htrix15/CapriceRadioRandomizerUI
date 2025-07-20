using HtmlAgilityPack;
using Infrastructure.Constants;

namespace Services;

public class CapricePageService
{
  
    public async Task<HtmlDocument> GetCapricePage()
    {
        var page = await new HtmlWeb().LoadFromWebAsync(CapricePageConstants.MainPagehUrl);
        return page ?? throw new Exception("MainPage not found!");
    }

    public HtmlNodeCollection SearchGenresTables(HtmlDocument mainPage)
    {
        return mainPage.DocumentNode.SelectNodes($"//table[contains(@id, '{CapricePageConstants.GenreTableIdSubStr}')]") ?? throw new Exception("Genres tables not found!");
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
}
