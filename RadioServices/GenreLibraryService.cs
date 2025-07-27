using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace RadioServices;

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

        remoteGenres.ForEach(g => g.ParentGenre = parentGenres.FirstOrDefault(pg => pg.Key == g.ParentGenreKey));

        return remoteGenres;
    }

    public async Task<List<Genre>> GetGenres()
    {
        var dbGenres = await genreRepository.GetAllGenres();
        
        if (dbGenres.Count == 0)
        {
            dbGenres = await CreateNewGenres();
        }

        return dbGenres;
    }
}
