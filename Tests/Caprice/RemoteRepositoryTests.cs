using Caprice;
using Infrastructure.Interfaces;
using Moq;

namespace Tests.Caprice;

internal class RemoteRepositoryTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetGenres_ReturnGenres()
    {
        var capricePageServiceMock = new Mock<PageService>();

        IRemoteRepository service = new Repository(capricePageServiceMock.Object,
            capricePageServiceMock.Object);
        
        var result = await service.GetGenres();

        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    public async Task GetTrackInfo_TrackNameIsNotNullNotEmpty()
    {
        var capricePageServiceMock = new Mock<PageService>();
        var trackInfoLink = "http://79.111.14.76:8000/status.xsl?mount=/darkwave";

        IRemoteRepository service = new Repository(capricePageServiceMock.Object,
            capricePageServiceMock.Object);

        var result = await service.GetTrackInfo(trackInfoLink);

        Assert.That(result.Name, Is.Not.Null.And.Not.Empty);
    }
}
