using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Repositories;

public class CapriceRepository : IRemoteRepository
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
