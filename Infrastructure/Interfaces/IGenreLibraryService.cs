using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IGenreLibraryService
{
    public Task<List<Genre>> GetGenres();
}
