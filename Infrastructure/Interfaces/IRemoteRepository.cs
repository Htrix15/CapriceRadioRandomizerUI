using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IRemoteRepository
{
    public Task<List<Genre>> GetGenres();
    public Task FillingGenres(List<Genre> genres);
    public TrackInfo GetTrackInfo(string trackInfoLink);
}
