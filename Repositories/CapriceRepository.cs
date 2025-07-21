using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Repositories;

public class CapriceRepository(IRemoteService remoteService) : IRemoteRepository
{
    public async Task<List<Genre>> GetGenres()
    {
        return await remoteService.CreateGenres();
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
