using Backend.Features.Board;
using Backend.Features.BoardCard;
using Backend.Features.Card;
using Backend.Features.Game;
using Backend.Features.Message;
using Backend.Features.Player;

namespace Backend.Features.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<PlayerEntity> Player { get; set; }
    public DbSet<GameEntity> Game { get; set; }
    public DbSet<MessageEntity> Message { get; set; }
    public DbSet<BoardEntity> Board { get; set; }
    public DbSet<CardEntity> Card { get; set; }
    public DbSet<BoardCardEntity> BoardCard { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseInMemoryDatabase("TestDatabase");
        }

        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlayerEntity>()
            .HasKey(p => p.ID);

        SetupGame(modelBuilder);
        SetupMessage(modelBuilder);
        SetupBoard(modelBuilder);
        SetupCard(modelBuilder);
        SetupBoardCard(modelBuilder);
    }

    private static void SetupGame(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GameEntity>()
            .HasKey(g => g.ID);

        modelBuilder.Entity<GameEntity>()
            .HasOne(g => g.PlayerOne)
            .WithMany(p => p.GamesAsPlayerOne)
            .HasForeignKey(g => g.PlayerOneID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GameEntity>()
            .HasOne(g => g.PlayerTwo)
            .WithMany(p => p.GamesAsPlayerTwo)
            .HasForeignKey(g => g.PlayerTwoID)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void SetupMessage(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MessageEntity>()
            .HasKey(m => m.ID);

        modelBuilder.Entity<MessageEntity>()
            .HasOne(m => m.Game)
            .WithMany(m => m.Messages)
            .HasForeignKey(m => m.GameID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MessageEntity>()
            .HasOne(m => m.Player)
            .WithMany(p => p.Messages)
            .HasForeignKey(m => m.PlayerID)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void SetupBoard(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BoardEntity>()
            .HasKey(b => b.ID);

        modelBuilder.Entity<BoardEntity>()
            .HasOne(b => b.Game)
            .WithMany(g => g.Boards)
            .HasForeignKey(b => b.GameID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BoardEntity>()
            .HasOne(b => b.Player)
            .WithMany(p => p.Boards)
            .HasForeignKey(b => b.PlayerID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BoardEntity>()
            .HasOne(b => b.ChosenCard)
            .WithMany()
            .HasForeignKey(b => b.ChosenCardID)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void SetupCard(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CardEntity>()
            .HasKey(c => c.ID);

        modelBuilder.Entity<CardEntity>()
            .HasOne(c => c.Player)
            .WithMany(p => p.Cards)
            .HasForeignKey(c => c.PlayerID)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void SetupBoardCard(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BoardCardEntity>()
            .HasKey(bc => bc.ID);

        modelBuilder.Entity<BoardCardEntity>()
            .HasOne(bc => bc.Card)
            .WithMany(c => c.BoardCards)
            .HasForeignKey(bc => bc.CardID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BoardCardEntity>()
            .HasOne(bc => bc.Board)
            .WithMany(b => b.BoardCards)
            .HasForeignKey(bc => bc.BoardID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}