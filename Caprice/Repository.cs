using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Caprice;

public class Repository(IRemoteService remoteService,
    IPageService pageService) : IRemoteRepository
{
    public async Task<List<Genre>> GetGenres()
    {
        return await remoteService.CreateGenres();
    }

    public Task FillingGenres(List<Genre> genres)
    {
        throw new NotImplementedException();
    }


    public async Task<TrackInfo> GetTrackInfo(string trackInfoLink)
    {
        var pageWithTrackInfo = await pageService.GetPage(trackInfoLink);
        var trackName = pageService.GetInnerTextByPath(pageWithTrackInfo, Constants.TrackNameXPath);
        return new TrackInfo()
        {
            Name = trackName,
        };
    }
}
