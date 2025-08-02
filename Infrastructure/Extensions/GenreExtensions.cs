using Infrastructure.Models;

namespace Infrastructure.Extensions;

public static class GenreExtensions
{
    public static decimal CalculatedRating(this Genre genre) => genre.ItIsParent
      ? genre.SubGenres?.Sum(CalculatedRating) ?? 0
      : (genre.Rating / (decimal)genre.RatingCount) * 100;
}
