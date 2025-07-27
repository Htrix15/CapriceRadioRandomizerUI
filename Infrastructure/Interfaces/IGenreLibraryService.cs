using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IGenreLibraryService
{
    public Task<List<Genre>> GetGenres();
    public Task SkipGenre(Genre perent, Genre subGenre);
    public Task IncreaseGenreTrackCount(string key);
    public Task ChangeRating(string key, int rating);
    public Task DisableGenre(string key);
}
