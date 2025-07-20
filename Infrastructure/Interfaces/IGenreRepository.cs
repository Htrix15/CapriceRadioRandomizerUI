using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IGenreRepository
{
    public Task<bool> IsExist();
    public Task Init();

    public Task<List<Genre>> GetAllGenres();
    public Task AddGenres(List<Genre> genres);
    public Task RemoveGenres(List<Genre> genres);

    public Task GenresIsAvailable(List<Genre> genres);
    public Task GenresIsNotAvailable(List<Genre> genres);

    public Task GenresIsDisabled(List<Genre> genres);
    public Task GenresIsEnabled(List<Genre> genres);

    public Task IncreaseTrackCount(string genreKey);
    public Task ChangeRating(string genreKey, int rating);
}
