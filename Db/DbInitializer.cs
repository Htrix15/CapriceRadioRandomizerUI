using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Db;

public class DbInitializer(ApplicationDbContext context) : IDbInitializer
{
    public void Initialize()
    {
        context.Database.Migrate();
    }
}
