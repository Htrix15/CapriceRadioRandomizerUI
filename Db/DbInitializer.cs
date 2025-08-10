using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Db;

public class DbInitializer(IApplicationDbContext<DatabaseFacade> context) : IDbInitializer
{
    public void Initialize()
    {
        if (context is ApplicationDbContext appContext)
        {
            appContext.Database.Migrate();
        }
    }
}
