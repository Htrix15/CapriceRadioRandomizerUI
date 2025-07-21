using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IRemoteService
{
    public Task<List<Genre>> CreateGenres();
}
