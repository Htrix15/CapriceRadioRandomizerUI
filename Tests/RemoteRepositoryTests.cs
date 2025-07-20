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
    public void GetGenres_ReturnGenres()
    {
        var capricePageServiceMock = new Mock<CapricePageService>();

        IRemoteRepository service = new CapriceRepository(capricePageServiceMock.Object);
        
        var result = service.GetGenres();

        Assert.That(result, Is.Not.Empty);
    }
}
