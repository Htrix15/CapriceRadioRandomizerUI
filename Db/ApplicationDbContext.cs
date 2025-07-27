using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Db;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<Genre> Genres { get; set; }

    public DbSet<RemoteSources> RemoteSources { get; set; }

    public ApplicationDbContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Radio.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>(
            builder =>
            {
                builder.HasKey(g => g.Key);
                builder.Property(g => g.Key).HasMaxLength(256);

                builder.Property(g => g.Name).HasMaxLength(256);

                builder
                   .HasMany(g => g.SubGenres)
                   .WithOne(g => g.ParentGenre)
                   .HasForeignKey(g => g.ParentGenreKey)
                   .OnDelete(DeleteBehavior.SetNull);
            });

        modelBuilder.Entity<RemoteSources>(
            builder =>
            {
                builder.HasKey(g => g.Key);
                builder.Property(g => g.Key).HasMaxLength(256);

                builder.Property(g => g.TrackInfoBaseLink).HasMaxLength(256);
                builder.Property(g => g.PlayLink).HasMaxLength(256);

                builder
                    .HasOne(rs => rs.Genre)
                    .WithOne(g => g.RemoteSources)
                    .HasForeignKey<RemoteSources>(rs => rs.Key)
                    .OnDelete(DeleteBehavior.Cascade);
            });
    }
}
