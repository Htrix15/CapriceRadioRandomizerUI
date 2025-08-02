using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using RadioServices.Services;

namespace RadioServices;

public static class Registrar
{
    public static void RegistrationServices(this IServiceCollection collection)
    {
        collection.AddDbContext<IApplicationDbContext, Db.ApplicationDbContext>();
        collection.AddSingleton<IUrlPlayer, Players.UrlPlayer>();
        collection.AddSingleton<IGenreRepository, Db.Repository>();
        collection.AddSingleton<IPlayerService, PlayerService>();
        collection.AddSingleton<IGenreLibraryService, GenreLibraryService>();
        collection.AddSingleton<IRemoteRepository, Caprice.Repository>();
        collection.AddSingleton<IRemoteService, Caprice.PageService>();
        collection.AddSingleton<IPageService, Caprice.PageService>();
        collection.AddSingleton<IRandomGenreService, RandomGenreService>();
    }
}
