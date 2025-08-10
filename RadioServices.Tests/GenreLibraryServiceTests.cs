using Db;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using RadioServices.Services;

namespace RadioServices.Tests;

internal class GenreLibraryServiceTests
{
    List<Genre> genres;

    private ApplicationDbContext CreateSqliteContext()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    private void InitGenresSamples()
    {
        var perantGenre = new Genre()
        {
            Key = "perant",
            Name = "perant genre",
            ItIsParent = true,
            IsAvailable = true,
            IsDisabled = false,
            IsSkip = false,
            TrackCount = 0,
            RatingCount = 0,
            Rating = 0
        };

        var subGenre1 = new Genre()
        {
            Key = "subGenre1",
            Name = "sub genre 1",
            ItIsParent = false,
            ParentGenreKey = perantGenre.Key,
            ParentGenre = perantGenre,
            IsAvailable = true,
            IsDisabled = false,
            IsSkip = false,
            TrackCount = 0,
            RatingCount = 0,
            Rating = 0,
        };
        subGenre1.RemoteSources = new RemoteSources()
        {
            Key = subGenre1.Key,
            PlayLink = "PlayLink1",
            TrackInfoBaseLink = "TrackInfoBaseLink1"
        };

        var subGenre2 = new Genre()
        {
            Key = "subGenre2",
            Name = "sub genre 2",
            ItIsParent = false,
            ParentGenreKey = perantGenre.Key,
            ParentGenre = perantGenre,
            IsAvailable = true,
            IsDisabled = false,
            IsSkip = false,
            TrackCount = 0,
            RatingCount = 0,
            Rating = 0
        };
        subGenre2.RemoteSources = new RemoteSources()
        {
            Key = subGenre2.Key,
            PlayLink = "PlayLink2",
            TrackInfoBaseLink = "TrackInfoBaseLink2"
        };
        genres = [perantGenre, subGenre1, subGenre2];
    }

    [SetUp]
    public void Setup()
    {

        InitGenresSamples();
    }

    [Test]
    public async Task RescanGenres_EmptyCheckedGenres_NotEmptyNewGenresEmptyUpdatedGernesDbNotEmpty()
    {
        using var memoryDbContex = CreateSqliteContext();
        var mockRemoteRepository = new Mock<IRemoteRepository>();
        var genreRepository = new Db.Repository(memoryDbContex);

        mockRemoteRepository
            .Setup(r => r.GetGenres())
            .ReturnsAsync(genres);

        var genreLibraryService = new GenreLibraryService(mockRemoteRepository.Object, genreRepository);

        List<Genre> newGenres;
        List<Genre> updatedGenres;

        (newGenres, updatedGenres) = await genreLibraryService.RescanGenres([]);

        var dbGenres = await genreRepository.GetAllGenres();

        var genresCount = 3; //1 perant + 2 sub;

        Assert.Multiple(() =>
        {
            Assert.That(newGenres, Has.Count.EqualTo(genresCount));
            Assert.That(updatedGenres, Has.Count.EqualTo(0));
            Assert.That(dbGenres, Has.Count.EqualTo(genresCount));
        });
    }

    [Test]
    public async Task RescanGenres_NotEmptyCheckedGenres_EmptyNewGenresNotEmptyUpdatedGernes()
    {
        using var memoryDbContex = CreateSqliteContext();
        var mockRemoteRepository = new Mock<IRemoteRepository>();
        var genreRepository = new Db.Repository(memoryDbContex);

        await genreRepository.AddGenres(genres);

        mockRemoteRepository
            .Setup(r => r.GetGenres())
            .ReturnsAsync(genres);

       var genreLibraryService = new GenreLibraryService(mockRemoteRepository.Object, genreRepository);

        List<Genre> newGenres;
        List<Genre> updatedGenres;

        (newGenres, updatedGenres) = await genreLibraryService.RescanGenres(genres);

        var dbGenres = await genreRepository.GetAllGenres();

        var genresCount = 3; //1 perant + 2 sub;

        Assert.Multiple(() =>
        {
            Assert.That(updatedGenres, Has.Count.EqualTo(genresCount));
            Assert.That(newGenres, Has.Count.EqualTo(0));
        });
    }

    [Test]
    public async Task RescanGenres_2CheckedGenres_EmptyNewGenresOneNotAvailableGenres()
    {
        using var memoryDbContex = CreateSqliteContext();
        var mockRemoteRepository = new Mock<IRemoteRepository>();
        var genreRepository = new Db.Repository(memoryDbContex);

        mockRemoteRepository
            .Setup(r => r.GetGenres())
            .ReturnsAsync(genres.Take(2).ToList());

        var genreLibraryService = new GenreLibraryService(mockRemoteRepository.Object, genreRepository);

        List<Genre> newGenres;
        List<Genre> updatedGenres;

        (newGenres, updatedGenres) = await genreLibraryService.RescanGenres(genres);

        Assert.Multiple(() =>
        {
            Assert.That(updatedGenres.Where(g => !g.IsAvailable).ToList(), Has.Count.EqualTo(1));
            Assert.That(newGenres, Has.Count.EqualTo(0));
        });
    }

    [Test]
    public async Task RescanGenres_EmptyCheckedGenresNewGenre_NotEmptyNewGenres()
    {
        using var memoryDbContex = CreateSqliteContext();
        var mockRemoteRepository = new Mock<IRemoteRepository>();
        var genreRepository = new Db.Repository(memoryDbContex);

        mockRemoteRepository
            .Setup(r => r.GetGenres())
            .ReturnsAsync(genres);

        var genreLibraryService = new GenreLibraryService(mockRemoteRepository.Object, genreRepository);

        List<Genre> newGenres;
        List<Genre> updatedGenres;

        (newGenres, updatedGenres) = await genreLibraryService.RescanGenres([]);

        var dbGenres = await genreRepository.GetAllGenres();

        var genresCount = 3; //1 perant + 2 sub;

        Assert.Multiple(() =>
        {
            Assert.That(updatedGenres, Has.Count.EqualTo(0));
            Assert.That(newGenres, Has.Count.EqualTo(genresCount));
            Assert.That(dbGenres, Has.Count.EqualTo(genresCount));
        });
    }
}
