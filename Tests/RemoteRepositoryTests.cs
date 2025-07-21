using Infrastructure.Interfaces;
using Moq;
using Repositories;
using Services;

namespace Tests;

internal class RemoteRepositoryTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetGenres_ReturnGenres()
    {
        var capricePageServiceMock = new Mock<CapricePageService>();

        IRemoteRepository service = new CapriceRepository(capricePageServiceMock.Object);
        
        var result = await service.GetGenres();

        Assert.That(result, Is.Not.Empty);
    }
}
