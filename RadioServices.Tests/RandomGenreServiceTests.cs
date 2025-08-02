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
    public void GetRandomGenre_PerantGenresWithDifferentChildRatings_PerantGenreWithMoreChildRatingsIsChosenMoreOfThen()
    {
        var popularPerantGenreKey = "Perant" + PopularGenreKey;
        var popularPerantGenre = new Genre()
        {
            Key = popularPerantGenreKey,
            Name = "popularPerantGenre",
            ItIsParent = true,
            IsAvailable = true,
            IsDisabled = false,
            IsSkip = false,
            TrackCount = 1,
            RatingCount = 0,
            Rating = 0,
            SubGenres = [PopularGenre, PopularGenre, PopularGenre, NeutralGenre, NotPopularGenre]
        };

        var neutralPerantGenreKey = "Perant" + NeutralGenreKey;
        var neutralPerantGenre = new Genre()
        {
            Key = neutralPerantGenreKey,
            Name = "neutralPerantGenre",
            ItIsParent = true,
            IsAvailable = true,
            IsDisabled = false,
            IsSkip = false,
            TrackCount = 1,
            RatingCount = 0,
            Rating = 0,
            SubGenres = [NeutralGenre, NeutralGenre, NeutralGenre, PopularGenre, NotPopularGenre]
        };

        var notPopularPerantGenreKey = "Perant" + NotPopularGenreKey;
        var notPopularPerantGenre = new Genre()
        {
            Key = notPopularPerantGenreKey,
            Name = "notPopularPerantGenre",
            ItIsParent = true,
            IsAvailable = true,
            IsDisabled = false,
            IsSkip = false,
            TrackCount = 1,
            RatingCount = 0,
            Rating = 0,
            SubGenres = [NotPopularGenre, NotPopularGenre, NotPopularGenre, NeutralGenre, PopularGenre]
        };


        List<Genre> genres = [popularPerantGenre, neutralPerantGenre, notPopularPerantGenre];

        IRandomGenreService randomService = new RandomGenreService();

        var statistics = new Dictionary<string, int>
        {
            { popularPerantGenreKey, 0 },
            { neutralPerantGenreKey, 0 },
            { notPopularPerantGenreKey, 0 },
        };

        for (var i = 0; i < 100000; i++)
        {
            var randomGenre = randomService.GetRandomGenre(Infrastructure.Enums.RandomeMode.PerantAndSubGenreWithRatingRange, null, genres).perant;
            statistics[randomGenre.Key]++;
        }

        Assert.Multiple(() =>
        {
            Assert.That(statistics[popularPerantGenreKey], Is.GreaterThan(statistics[neutralPerantGenreKey]));
            Assert.That(statistics[neutralPerantGenreKey], Is.GreaterThan(statistics[notPopularPerantGenreKey]));
        });
    }
}
