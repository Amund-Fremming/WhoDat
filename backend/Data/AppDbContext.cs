using Microsoft.EntityFrameworkCore;

using PlayerEntity;
using GalleryEntity;
using GameEntity;
using MessageEntity;
using BoardEntity;
using CardEntity;
using BoardCardEntity;

namespace Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Player> Player { get; set; }
    public DbSet<Gallery> Gallery { get; set; }
    public DbSet<Game> Game { get; set; }
    public DbSet<Message> Message { get; set; }
    public DbSet<Board> Board { get; set; }
    public DbSet<Card> Card { get; set; }
    public DbSet<BoardCard> BoardCard { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>()
            .HasKey(p => p.PlayerID);

        modelBuilder.Entity<Gallery>()
            .HasKey(g => g.GalleryID);

        modelBuilder.Entity<Gallery>()
            .HasOne(g => g.Player)
            .WithOne(p => p.Gallery)
            .HasForeignKey<Gallery>(g => g.PlayerID)
            .OnDelete(DeleteBehavior.Cascade);





        modelBuilder.Entity<>();
    }
}
