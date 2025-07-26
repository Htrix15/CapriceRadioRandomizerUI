using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Db;

public class Repository : IGenreRepository
{
    public Task AddGenres(List<Genre> genres)
    {
        throw new NotImplementedException();
    }

    public Task ChangeRating(string genreKey, int rating)
    {
        throw new NotImplementedException();
    }

    public Task GenresIsAvailable(List<Genre> genres)
    {
        throw new NotImplementedException();
    }

    public Task GenresIsDisabled(List<Genre> genres)
    {
        throw new NotImplementedException();
    }

    public Task GenresIsEnabled(List<Genre> genres)
    {
        throw new NotImplementedException();
    }

    public Task GenresIsNotAvailable(List<Genre> genres)
    {
        throw new NotImplementedException();
    }

    public Task<List<Genre>> GetAllGenres()
    {
        throw new NotImplementedException();
    }

    public Task IncreaseTrackCount(string genreKey)
    {
        throw new NotImplementedException();
    }

    public Task Init()
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsExist()
    {
        throw new NotImplementedException();
    }

    public Task RemoveGenres(List<Genre> genres)
    {
        throw new NotImplementedException();
    }
}
