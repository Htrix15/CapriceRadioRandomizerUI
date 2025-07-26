using Caprice;
using Infrastructure.Interfaces;
using Moq;

namespace Tests.Caprice;

internal class RepositoryTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetGenres_ReturnGenres()
    {
        var capricePageServiceMock = new Mock<PageService>();

        IRemoteRepository service = new Repository(capricePageServiceMock.Object);
        
        var result = await service.GetGenres();

        Assert.That(result, Is.Not.Empty);
    }
}
