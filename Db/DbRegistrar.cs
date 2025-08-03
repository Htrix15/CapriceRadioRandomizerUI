using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Db;

public static class DbRegistrar
{
    public static void RegistrationDb(this IServiceCollection collection)
    {
        var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Radio.db");
        var connectionString = $"Data Source={dbPath}";

        collection.AddDbContext<IApplicationDbContext, Db.ApplicationDbContext>(options =>
            options.UseSqlite(connectionString)
        );
    }
}
