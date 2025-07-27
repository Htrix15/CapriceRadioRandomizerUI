using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Db;

public class Repository(IApplicationDbContext dbContext) : IGenreRepository
{
    public async Task AddGenres(List<Genre> genres)
    {
        await dbContext.Genres.AddRangeAsync(genres);
        await dbContext.SaveChangesAsync();
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

    public async Task<List<Genre>> GetAllGenres()
    {
        return await dbContext.Genres
            .AsNoTracking()
            .Include(g => g.RemoteSources)
            .ToListAsync();
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
