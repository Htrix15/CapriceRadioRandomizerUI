using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Options;

namespace RadioServices.Services;

public class GenreLibraryService(IRemoteRepository remoteRepository,
    IGenreRepository genreRepository) : IGenreLibraryService
{
    private async Task<List<Genre>> CreateNewGenres()
    {
        var remoteGenres = await remoteRepository.GetGenres();

        var parentGenres = remoteGenres
            .Select(g => g.ParentGenre)
            .DistinctBy(g => g.Key)
            .ToList();
        await genreRepository.AddGenres(parentGenres!);

        remoteGenres.ForEach(g => g.ParentGenre = null);
        await genreRepository.AddGenres(remoteGenres!);

        remoteGenres.ForEach(g => g.ParentGenre = parentGenres.First(pg => pg.Key == g.ParentGenreKey));

        return [..parentGenres, ..remoteGenres];
    }

    public async Task<List<Genre>> GetGenres()
    {
        var genres = await genreRepository.GetAllActiveGenres();
        
        if (genres.Count == 0)
        {
            genres = await CreateNewGenres();
        }

        var perantGenres = genres.Where(g => g.ItIsParent).ToList();

        foreach (var perantGenre in perantGenres)
        {
            perantGenre.SubGenres = genres.Where(g => g.ParentGenreKey == perantGenre.Key).ToList();
        }

        return perantGenres;
    }

    public async Task SkipGenre(Genre perent, Genre subGenre)
    {
        await genreRepository.SkipGenre(subGenre.Key);
        subGenre.IsSkip = true;

        perent.SubGenres = [.. perent.SubGenres!.Where(sg => !sg.IsSkip)];

        if (perent.SubGenres!.All(sg => sg.IsSkip))
        {
            await genreRepository.SkipGenre(perent.Key);
            perent.IsSkip = true;
        }
    }

    public async Task IncreaseGenreTrackCount(string key)
    {
        await genreRepository.IncreaseTrackCount(key);
    }

    public async Task ChangeRating(string genreKey, int rating)
    {
        await genreRepository.ChangeRating(genreKey, rating);
    }

    public async Task DisableGenre(string key)
    {
        await genreRepository.DisableGenre(key);
    }

    public async Task ReActiveteSubGenres(string parentGanreKey)
    {
        await genreRepository.ActivateSubGenres(parentGanreKey);
    }

    public async Task ItIsLastChoice(string genreKey)
    {
        await genreRepository.ItIsLastChoice(genreKey);
    }

    public async Task UpdateGenres(List<Genre> genres, UpdateGenreOptions options)
    {
        await genreRepository.UpdateGenres(genres, options);
    }


    public List<Genre> BuildAllGenresExludedSubGenresSubjection(List<Genre> genres)
    {
        var perantGenres = genres.Where(g => g.ItIsParent).ToList();
        var childGenres = perantGenres.SelectMany(pg => pg.SubGenres!).ToList();
        perantGenres.ForEach(p => p.SubGenres = null);
        return [..perantGenres, ..childGenres];
    }

    public List<Genre> BuildAllGenresExludedPerantSubjection(List<Genre> genres)
    {
        var perantGenres = genres.Where(g => g.ParentGenre != null)
            .Select(g => g.ParentGenre)
            .DistinctBy(g => g.Key)
            .ToList();

        genres.ForEach(p => p.ParentGenre = null);
        return [.. perantGenres, .. genres];
    }

    public async Task<(List<Genre> New, List<Genre> Updated)> RescanGenres(List<Genre> genres)
    {
        var newGenres = new List<Genre>();
        var updatedGenres = new List<Genre>();

        var remoteGenres = BuildAllGenresExludedPerantSubjection(await remoteRepository.GetGenres());

        var remoteGenresKeys = remoteGenres.Select(g => g.Key).ToList();
        var notAvailableGenres = genres.Where(g => !remoteGenresKeys.Contains(g.Key)).ToList();
        if (notAvailableGenres.Count > 0)
        {
            notAvailableGenres.ForEach(g => g.IsAvailable = false);
            await genreRepository.UpdateGenres(notAvailableGenres, 
                new UpdateGenreOptions() { UpdateIsAvailable = true});
            updatedGenres.AddRange(notAvailableGenres);
        }

        var oldGenresKey = genres.Select(g => g.Key).ToList();
        newGenres = remoteGenres.Where(g => !oldGenresKey.Contains(g.Key)).ToList();
        if (newGenres.Count > 0)
        {
            var perantGenres = newGenres.Where(g => g.ItIsParent).ToList();
            await genreRepository.AddGenres(perantGenres);

            var subGenres = newGenres.Where(g => !g.ItIsParent).ToList();
            await genreRepository.AddGenres(subGenres);
        }

        var updatedGenre = remoteGenres.Where(g => oldGenresKey.Contains(g.Key)).ToList();
        if (updatedGenre.Count > 0)
        {
            await genreRepository.UpdateGenres(updatedGenre,
               new UpdateGenreOptions() { 
                   UpdateName = true,
                   UpdateRemoteSources = true,
                   UpdatePerantKey = true,
               });
            updatedGenres.AddRange(updatedGenre);
        }

        return (newGenres, updatedGenres);
    }
}
