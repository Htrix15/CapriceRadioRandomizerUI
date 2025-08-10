using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace UI;

public class GenresViewerFormFactory(IServiceProvider serviceProvider, IGenreLibraryService genreLibrary)
{
    public async Task<GenresViewerForm> Create()
    {
        var genreLibraryService = serviceProvider.GetRequiredService<IGenreLibraryService>();
        var allGenres = await genreLibrary.GetAllGenres();
        return new GenresViewerForm(genreLibraryService, allGenres);
    }
}
