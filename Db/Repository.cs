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

    public async Task ChangeRating(string genreKey, int rating)
    {
        var updatedGenre = await dbContext.Genres.FindAsync(genreKey);
        updatedGenre!.Rating += rating;
        updatedGenre.RatingCount++;
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<Genre>> GetAllActiveGenres()
    {
        return await dbContext.Genres
            .AsNoTracking()
            .Where(g => g.IsAvailable 
                && !g.IsDisabled
                && !g.IsSkip)
            .Include(g => g.RemoteSources)
            .ToListAsync();
    }

    public async Task IncreaseTrackCount(string genreKey)
    {
        var updatedGenre = await dbContext.Genres.FindAsync(genreKey);
        updatedGenre!.TrackCount++;
        await dbContext.SaveChangesAsync();
    }

    public async Task SkipGenre(string genreKey)
    {
        var updatedGenre = await dbContext.Genres.FindAsync(genreKey);
        updatedGenre!.IsSkip = true;
        await dbContext.SaveChangesAsync();
    }

    public async Task DisableGenre(string genreKey)
    {
        var updatedGenre = await dbContext.Genres.FindAsync(genreKey);
        updatedGenre!.IsDisabled = true;
        await dbContext.SaveChangesAsync();
    }
}
