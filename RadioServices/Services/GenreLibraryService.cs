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
            .Where(g => g.ItIsParent)
            .ToList();

        var subGenres = remoteGenres
            .Where(g => !g.ItIsParent)
            .ToList();

        await genreRepository.AddAndUpdateGenresInTransaction(newGenres: [parentGenres, subGenres]);

        return remoteGenres;
    }

    public async Task<List<Genre>> GetOrCreateGenres()
    {
        var genres = await genreRepository.GetAllActiveGenres();
        
        if (genres.Count == 0)
        {
            genres = await CreateNewGenres();
        }

        var perantGenres = genres.Where(g => g.ItIsParent).ToList();

        foreach (var perantGenre in perantGenres.Where(g => g.SubGenres == null || g.SubGenres.Count == 0))
        {
            perantGenre.SubGenres = [.. genres.Where(g => g.ParentGenreKey == perantGenre.Key)];
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

    public async Task<(List<Genre> New, List<Genre> Updated)> RescanGenres(List<Genre> checkedGenres)
    {
        var updatedGenres = new List<(UpdateGenreOptions updateGenreOptions, List<Genre> genres)>();
        var addGenres = new List<List<Genre>>();

        var remoteGenres = await remoteRepository.GetGenres();

        var remoteGenresKeys = remoteGenres.Select(g => g.Key).ToList();
        var notAvailableGenres = checkedGenres.Where(g => !remoteGenresKeys.Contains(g.Key)).ToList();
        if (notAvailableGenres.Count > 0)
        {
            notAvailableGenres.ForEach(g => g.IsAvailable = false);
            updatedGenres.Add((new UpdateGenreOptions() { UpdateIsAvailable = true}, notAvailableGenres));
        }

        var oldGenresKey = checkedGenres.Select(g => g.Key).ToList();
        var newGenres = remoteGenres.Where(g => !oldGenresKey.Contains(g.Key)).ToList();

        if (newGenres.Count > 0)
        {
            var perantGenres = newGenres.Where(g => g.ItIsParent).ToList();
            addGenres.Add(perantGenres);

            var subGenres = newGenres.Where(g => !g.ItIsParent).ToList();
            addGenres.Add(subGenres);
        }

        var updatedGenre = remoteGenres.Where(g => oldGenresKey.Contains(g.Key)).ToList();
        if (updatedGenre.Count > 0)
        {
            updatedGenres.Add((new UpdateGenreOptions()
            {
                UpdateName = true,
                UpdateRemoteSources = true,
                UpdateParentKey = true,
            }, updatedGenre));
        }

        await genreRepository.AddAndUpdateGenresInTransaction(addGenres, updatedGenres);

        return ([.. addGenres.SelectMany(g => g)], 
            [..updatedGenres.SelectMany(g => g.genres)
            .DistinctBy(g => g.Key)]);
    }

    public async Task<List<Genre>> GetAllGenres()
    {
        return await genreRepository.GetAllGenres();
    }
}
