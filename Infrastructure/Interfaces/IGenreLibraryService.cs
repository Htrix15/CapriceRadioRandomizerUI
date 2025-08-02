using Infrastructure.Models;
using Infrastructure.Options;

namespace Infrastructure.Interfaces;

public interface IGenreLibraryService
{
    public Task<List<Genre>> GetGenres();
    public Task UpdateGenres(List<Genre> genres, UpdateGenreOptions options);
    public Task SkipGenre(Genre perent, Genre subGenre);
    public Task IncreaseGenreTrackCount(string key);
    public Task ChangeRating(string key, int rating);
    public Task DisableGenre(string key);
    public Task ReActiveteSubGenres(string parentGanreKey);
    public Task ItIsLastChoice(string genreKey);
}
