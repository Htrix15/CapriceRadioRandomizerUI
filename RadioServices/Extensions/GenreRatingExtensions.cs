using Infrastructure.Models;

namespace RadioServices.Extensions;

public static class GenreRatingExtensions
{
    public static decimal CalculatedProportionalRating(this Genre genre) => genre.ItIsParent
      ? genre.SubGenres?.Sum(CalculatedProportionalRating) ?? 0
      : genre.Rating / (decimal)genre.RatingCount * 100;
}
