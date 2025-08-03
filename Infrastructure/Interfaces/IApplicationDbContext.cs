using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Interfaces;

public interface IApplicationDbContext : IDisposable
{
    public DbSet<Genre> Genres { get; set; }
    public DbSet<RemoteSources> RemoteSources { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
