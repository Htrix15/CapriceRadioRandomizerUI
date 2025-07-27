using HtmlAgilityPack;

namespace Infrastructure.Interfaces;

public interface IPageService
{
    public Task<HtmlDocument> GetPage(string url);
    public string GetInnerTextByPath(HtmlDocument page, string path);
}
