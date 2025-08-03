using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IRemoteRepository
{
    public Task<List<Genre>> GetGenres();
    public Task <TrackInfo> GetTrackInfo(string trackInfoLink);
}
