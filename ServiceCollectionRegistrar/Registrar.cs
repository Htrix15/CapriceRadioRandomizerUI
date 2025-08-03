using Db;
using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceCollectionRegistrar;

public static class Registrar
{
    public static void Registration(this IServiceCollection collection)
    {
        collection.RegistrationDb();
        collection.AddScoped<IDbInitializer, Db.DbInitializer>();
        collection.AddSingleton<IGenreRepository, Db.Repository>();
        collection.AddSingleton<IUrlPlayer, Players.UrlPlayer>();
        collection.AddSingleton<IPlayerService, RadioServices.Services.PlayerService>();
        collection.AddSingleton<IGenreLibraryService, RadioServices.Services.GenreLibraryService>();
        collection.AddSingleton<IRemoteRepository, Caprice.Repository>();
        collection.AddSingleton<IRemoteService, Caprice.PageService>();
        collection.AddSingleton<IPageService, Caprice.PageService>();
        collection.AddSingleton<IRandomGenreService, RadioServices.Services.RandomGenreService>();
    }
}
