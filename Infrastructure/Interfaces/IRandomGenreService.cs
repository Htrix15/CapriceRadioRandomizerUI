using Infrastructure.Enums;
using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IRandomGenreService
{
    public Genre GetRandomGenre(List<Genre> genres);
    public Genre GetRandomWithRatingRange(List<Genre> genres);
    public (Genre perant, Genre sub) GetRandomGenre(RandomeMode randomeMode, Genre currentGenre, List<Genre> allPerantGenre);
}
