using Infrastructure.Interfaces;
using Infrastructure.Models;
using RadioServices.Services;

namespace RadioServices.Tests;

internal class RandomGenreServiceTests
{

    private const string PopularGenreKey = "PopularGenre";
    private Genre PopularGenre;

    private const string NotPopularGenreKey = "NotPopularGenre";
    private Genre NotPopularGenre;

    private const string NeutralGenreKey = "NeutralGenre";
    private Genre NeutralGenre;

    [SetUp]
    public void Setup()
    {
        PopularGenre = new Genre()
        {
            Key = PopularGenreKey,
            Name = "PopularGenre",
            ItIsParent = false,
            IsAvailable = true,
            IsDisabled = false,
            IsSkip = false,
            TrackCount = 1,
            RatingCount = 100,
            Rating = 90,
        };

        NotPopularGenre = new Genre()
        {
            Key = NotPopularGenreKey,
            Name = "NotPopularGenre",
            ItIsParent = false,
            IsAvailable = true,
            IsDisabled = false,
            IsSkip = false,
            TrackCount = 1,
            RatingCount = 100,
            Rating = -90,
        };

        NeutralGenre = new Genre()
        {
            Key = NeutralGenreKey,
            Name = "NeutralGenre",
            ItIsParent = false,
            IsAvailable = true,
            IsDisabled = false,
            IsSkip = false,
            TrackCount = 1,
            RatingCount = 100,
            Rating = 0,
        };
    }

    [Test]
    public void GetRandomWithRatingRange_GenresWithDifferentRating_GenreWithMoreRatingIsChosenMoreOfThen()
    {
        List<Genre> genres = [NotPopularGenre, PopularGenre, NeutralGenre];

        IRandomGenreService randomService = new RandomGenreService();

        var statistics = new Dictionary<string, int>
        {
            { NeutralGenreKey, 0 },
            { NotPopularGenreKey, 0 },
            { PopularGenreKey, 0 },
        };

        for (var i = 0; i < 100000; i++)
        {
            var randomGenre = randomService.GetRandomWithRatingRange(genres);
            statistics[randomGenre.Key]++;
        }

        Assert.Multiple(() =>
        {
            Assert.That(statistics[PopularGenreKey], Is.GreaterThan(statistics[NeutralGenreKey]));
            Assert.That(statistics[NeutralGenreKey], Is.GreaterThan(statistics[NotPopularGenreKey]));
        });
    }

    [Test]
    public void GetRandomGenre_ParentGenresWithDifferentChildRatings_ParentGenreWithMoreChildRatingsIsChosenMoreOfThen()
    {
        var popularParentGenreKey = "Parent" + PopularGenreKey;
        var popularParentGenre = new Genre()
        {
            Key = popularParentGenreKey,
            Name = "popularParentGenre",
            ItIsParent = true,
            IsAvailable = true,
            IsDisabled = false,
            IsSkip = false,
            TrackCount = 1,
            RatingCount = 0,
            Rating = 0,
            SubGenres = [PopularGenre, PopularGenre, PopularGenre, NeutralGenre, NotPopularGenre]
        };

        var neutralParentGenreKey = "Parent" + NeutralGenreKey;
        var neutralParentGenre = new Genre()
        {
            Key = neutralParentGenreKey,
            Name = "neutralParentGenre",
            ItIsParent = true,
            IsAvailable = true,
            IsDisabled = false,
            IsSkip = false,
            TrackCount = 1,
            RatingCount = 0,
            Rating = 0,
            SubGenres = [NeutralGenre, NeutralGenre, NeutralGenre, PopularGenre, NotPopularGenre]
        };

        var notPopularParentGenreKey = "Parent" + NotPopularGenreKey;
        var notPopularParentGenre = new Genre()
        {
            Key = notPopularParentGenreKey,
            Name = "notPopularParentGenre",
            ItIsParent = true,
            IsAvailable = true,
            IsDisabled = false,
            IsSkip = false,
            TrackCount = 1,
            RatingCount = 0,
            Rating = 0,
            SubGenres = [NotPopularGenre, NotPopularGenre, NotPopularGenre, NeutralGenre, PopularGenre]
        };


        List<Genre> genres = [popularParentGenre, neutralParentGenre, notPopularParentGenre];

        IRandomGenreService randomService = new RandomGenreService();

        var statistics = new Dictionary<string, int>
        {
            { popularParentGenreKey, 0 },
            { neutralParentGenreKey, 0 },
            { notPopularParentGenreKey, 0 },
        };

        for (var i = 0; i < 100000; i++)
        {
            var randomGenre = randomService.GetRandomGenre(Infrastructure.Enums.RandomeMode.ParentAndSubGenreWithRatingRange, null, genres).parent;
            statistics[randomGenre.Key]++;
        }

        Assert.Multiple(() =>
        {
            Assert.That(statistics[popularParentGenreKey], Is.GreaterThan(statistics[neutralParentGenreKey]));
            Assert.That(statistics[neutralParentGenreKey], Is.GreaterThan(statistics[notPopularParentGenreKey]));
        });
    }
}
