using Infrastructure.Interfaces;
using Infrastructure.Models;
using RadioServices.Extensions;

namespace RadioServices.Services;

public partial class RandomGenreService : IRandomGenreService
{
    public class RatingProportionalGenre(Genre genre)
    {
        public Genre Genre = genre;
        public decimal ProportionalRating { get; set; }

    }

    private void RatingsPositiveOffseting(List<RatingProportionalGenre> genresRatings)
    {
        var minRating = genresRatings.Min(g => g.ProportionalRating);
        var offset = minRating < 0 ? -minRating + 1 : 1;
        genresRatings.ForEach(g => g.ProportionalRating += offset);
    }

    public List<RatingProportionalGenre> CalculateGenresProportionalRating(List<Genre> genres)
    {
        var result = genres.Select(g => new RatingProportionalGenre(g)).ToList();

        result.ForEach(r => r.ProportionalRating = r.Genre.CalculatedProportionalRating());

        if (result.All(r => r.ProportionalRating > 0)) return result;
        RatingsPositiveOffseting(result);
        return result;
    }

    public Genre GetRatingProportionalGenre(List<RatingProportionalGenre> genres)
    {
        var random = new Random();
        var totalWeight = genres.Sum(g => g.ProportionalRating);

        var cumulativeWeight = 0m;
        decimal randomValue = (decimal)random.NextDouble() * totalWeight;

        for (int i = 0; i < genres.Count; i++)
        {
            cumulativeWeight += genres[i].ProportionalRating;
            if (randomValue <= cumulativeWeight)
                return genres[i].Genre;
        }
        return genres.Last().Genre;
    }

    public Genre GetRandomWithRatingRange(List<Genre> genres)
    {
        var ratingProportionalGenre = CalculateGenresProportionalRating(genres);
        if (ratingProportionalGenre.All(r => r.ProportionalRating == 0)) return GetRandomGenre(genres);
        return GetRatingProportionalGenre(ratingProportionalGenre);
    }

    private (Genre perant, Genre sub) GetRandomSubGenreRatingRange(Genre currentGenre)
    {
        return (currentGenre, GetRandomWithRatingRange(currentGenre.SubGenres!));
    }

    private (Genre perant, Genre sub) GetRandomPerantAndSubGenreRatingRange(List<Genre> allPerantGenre)
    {
        var newPerant = GetRandomWithRatingRange(allPerantGenre);
        return (newPerant, GetRandomWithRatingRange(newPerant.SubGenres!));
    }
}