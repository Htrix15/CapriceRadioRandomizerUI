using Infrastructure.Enums;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace RadioServices.Services;

public partial class RandomGenreService : IRandomGenreService
{
    
    public Genre GetRandomGenre(List<Genre> genres)
    {
        var random = new Random();
        int randomIndex = random.Next(genres.Count);
        return genres[randomIndex];
    }

    private (Genre parent, Genre sub) GetRandomSubGenre(Genre currentGenre)
    {
        return (currentGenre, GetRandomGenre(currentGenre.SubGenres!));
    }

    private (Genre parent, Genre sub) GetRandomParentAndSubGenre(List<Genre> allParentGenre)
    {
        var newParent = GetRandomGenre(allParentGenre);
        return (newParent, GetRandomGenre(newParent.SubGenres!));
    }

    public (Genre parent, Genre sub) GetRandomGenre(RandomeMode randomeMode,
        Genre currentGenre,
        List<Genre> allParentGenre) => randomeMode switch
        {
            RandomeMode.SubGenre => GetRandomSubGenre(currentGenre),
            RandomeMode.ParentAndSubGenre => GetRandomParentAndSubGenre(allParentGenre),
            RandomeMode.SubGenreWithRatingRange => GetRandomSubGenreRatingRange(currentGenre),
            RandomeMode.ParentAndSubGenreWithRatingRange => GetRandomParentAndSubGenreRatingRange(allParentGenre),
            _ => throw new NotImplementedException(),
        };
  
}
