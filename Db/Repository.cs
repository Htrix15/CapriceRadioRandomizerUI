using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Options;
using Microsoft.EntityFrameworkCore;

namespace Db;

public class Repository(IApplicationDbContext dbContext) : IGenreRepository
{
    public async Task<List<Genre>> GetAllGenres()
    {
        return await dbContext.Genres
            .AsNoTracking()
            .Include(g => g.RemoteSources)
            .ToListAsync();
    }

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

    public async Task ActivateSubGenres(string perantGenreKey)
    {
        await dbContext.Genres.Where(g => g.ParentGenreKey == perantGenreKey)
            .ExecuteUpdateAsync(g =>
                g.SetProperty(p => p.IsDisabled, p => false));
    }

    public async Task ItIsLastChoice(string genreKey)
    {
        await dbContext.Genres.Where(g => g.Key != genreKey).ExecuteUpdateAsync(g => g.SetProperty(p => p.IsLastChoice, p => false));
        var updatedGenre = await dbContext.Genres.FindAsync(genreKey);
        updatedGenre!.IsLastChoice = true;
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateGenres(List<Genre> genres, UpdateGenreOptions options)
    {
        var query = dbContext.Genres.AsQueryable();
        if (options.UpdateRemoteSources)
        {
            query = query.Include(q => q.RemoteSources);
        }

        var allGenres = await query.ToListAsync();

        foreach (var genre in allGenres)
        {
            var genreUpdates = genres.FirstOrDefault(g => g.Key == genre.Key);
            if (genreUpdates == null) continue;

            if (options.UpdateName)
            {
                genre.Name = genreUpdates.Name;
            }

            if (options.UpdateIsAvailable)
            {
                genre.IsAvailable = genreUpdates.IsAvailable;
            }

            if (options.UpdateIsDisabled)
            {
                genre.IsDisabled = genreUpdates.IsDisabled;
            }

            if (options.UpdateIsSkip)
            {
                genre.IsSkip = genreUpdates.IsSkip;
            }

            if (options.UpdateRating)
            {
                genre.Rating = genreUpdates.Rating;
            }

            if (options.UpdateRemoteSources 
                && genreUpdates.RemoteSources != null
                && genre.RemoteSources != null)
            {
                genre.RemoteSources.TrackInfoBaseLink = genreUpdates.RemoteSources.TrackInfoBaseLink;
                genre.RemoteSources.PlayLink = genreUpdates.RemoteSources.PlayLink;
            }

            if (options.UpdatePerantKey)
            {
                genre.ParentGenreKey = genreUpdates.ParentGenreKey;
            }
        }

        await dbContext.SaveChangesAsync();
    }
}
