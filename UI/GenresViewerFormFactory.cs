using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;

namespace UI;

public class GenresViewerFormFactory(IServiceProvider serviceProvider)
{
    public GenresViewerForm Create(List<Genre> genres)
    {
        var genreLibraryService = serviceProvider.GetRequiredService<IGenreLibraryService>();

        return new GenresViewerForm(genreLibraryService, genres);
    }
}
