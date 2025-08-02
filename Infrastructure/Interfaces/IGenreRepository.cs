using Infrastructure.Models;
using Infrastructure.Options;

namespace Infrastructure.Interfaces;

public interface IGenreRepository
{
    public Task SkipGenre(string genreKey);
    public Task<List<Genre>> GetAllActiveGenres();
    public Task AddGenres(List<Genre> genres);
    public Task DisableGenre(string genreKey);
    public Task ActivateSubGenres(string perantGenreKey);
    public Task IncreaseTrackCount(string genreKey);
    public Task ChangeRating(string genreKey, int rating);
    public Task ItIsLastChoice(string genreKey);
    public Task UpdateGenres(List<Genre> genres, UpdateGenreOptions options);
}
