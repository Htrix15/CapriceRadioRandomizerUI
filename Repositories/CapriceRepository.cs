using Infrastructure.Interfaces;
using Infrastructure.Models;
using Services;

namespace Repositories;

public class CapriceRepository(CapricePageService capricePageService) : IRemoteRepository
{
    public Task<List<Genre>> GetGenres()
    {
        throw new NotImplementedException();
    }

    public Task FillingGenres(List<Genre> genres)
    {
        throw new NotImplementedException();
    }

    public TrackInfo GetTrackInfo(string trackInfoLink)
    {
        throw new NotImplementedException();
    }
}
