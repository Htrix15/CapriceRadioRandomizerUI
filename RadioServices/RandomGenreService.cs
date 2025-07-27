using Infrastructure.Enums;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace RadioServices;

public class RandomGenreService : IRandomGenreService
{
    public Genre GetRandomGenre(List<Genre> genres)
    {
        var random = new Random();
        int randomIndex = random.Next(genres.Count);
        return genres[randomIndex];
    }

    private (Genre perant, Genre sub) GetRandomSubGenre(Genre currentGenre)
    {
        return (currentGenre, GetRandomGenre(currentGenre.SubGenres!));
    }

    private (Genre perant, Genre sub) GetRandomPerantAndSubGenre(List<Genre> allPerantGenre)
    {
        var newPerant = GetRandomGenre(allPerantGenre);
        return (newPerant, GetRandomGenre(newPerant.SubGenres!));
    }

    public (Genre perant, Genre sub) GetRandomGenre(RandomeMode randomeMode,
        Genre currentGenre,
        List<Genre> allPerantGenre) => randomeMode switch
        {
            RandomeMode.SubGenre => GetRandomSubGenre(currentGenre),
            RandomeMode.PerantAndSubGenre => GetRandomPerantAndSubGenre(allPerantGenre),
            RandomeMode.SubGenreWithRatingRange => throw new NotImplementedException(),
            RandomeMode.PerantAndSubGenreWithRatingRange => throw new NotImplementedException(),
            _ => throw new NotImplementedException(),
        };
  
}
