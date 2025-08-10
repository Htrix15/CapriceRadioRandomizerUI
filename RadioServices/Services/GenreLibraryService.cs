using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Options;

namespace RadioServices.Services;

public class GenreLibraryService(IRemoteRepository remoteRepository,
    IGenreRepository genreRepository) : IGenreLibraryService
{
    public object genreLibraryService { get; private set; }

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

        var parentGenres = PackGenresIntoParentGenres(genres);

        parentGenres = [.. parentGenres.Where(p => p.SubGenres != null 
            && p.SubGenres.Count > 0 
            && (p.SubGenres!.All(sg => !sg.IsDisabled) 
                || p.SubGenres!.All(sg => !sg.IsSkip)))];

        parentGenres.ForEach(pg => pg.SubGenres = [.. pg.SubGenres!.Where(sg => !sg.IsDisabled && !sg.IsSkip)]);

        return parentGenres;
    }

    public async Task SkipGenre(Genre perent, Genre subGenre)
    {
        await genreRepository.SkipGenre(subGenre.Key);
        subGenre.IsSkip = true;

        perent.SubGenres = [.. perent.SubGenres!.Where(sg => !sg.IsSkip)];

        if (perent.SubGenres.Count == 0)
        {
            await genreRepository.ReskipSubGenres(perent.Key);
            perent.SubGenres = await genreRepository.GetAllSubGenres(perent.Key);
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
            var parentGenres = newGenres.Where(g => g.ItIsParent).ToList();
            addGenres.Add(parentGenres);

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

    public async Task UpdateGenres(List<Genre> genres)
    {
        var (newGenres, updatedGenres) = await RescanGenres(genres);
        if (newGenres.Count > 0)
        {
            genres.AddRange(newGenres);
        }

        if (updatedGenres.Count > 0)
        {
            var updatedGenresDictionary = updatedGenres.ToDictionary(g => g.Key, g => g);
            foreach (var genre in genres)
            {
                if (updatedGenresDictionary.TryGetValue(genre.Key, out var updatedGenre))
                {
                    genre.IsAvailable = updatedGenre.IsAvailable;
                    genre.Name = updatedGenre.Name;
                    genre.RemoteSources = updatedGenre.RemoteSources;
                    genre.ParentGenreKey = updatedGenre.ParentGenreKey;
                }
            }
        }
    }

    public async Task<List<Genre>> GetAllGenres()
    {
        return await genreRepository.GetAllGenres();
    }

    public List<Genre> PackGenresIntoParentGenres(List<Genre> allGenres)
    {
        var parentGenres = allGenres.Where(g => g.ItIsParent).ToList();
        parentGenres.ForEach(pg => pg.SubGenres = [.. allGenres.Where(g => g.ParentGenreKey == pg.Key)]);
        return parentGenres;
    }
}
