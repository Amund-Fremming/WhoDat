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
            .HasKey(p => p.PlayerID);

        setupGame(modelBuilder);
        setupMessage(modelBuilder);
        setupBoard(modelBuilder);
        setupCard(modelBuilder);
        setupBoardCard(modelBuilder);
    }

    private void setupGame(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GameEntity>()
            .HasKey(g => g.GameID);

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

    private void setupMessage(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MessageEntity>()
            .HasKey(m => m.MessageID);

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

    private void setupBoard(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BoardEntity>()
            .HasKey(b => b.BoardID);

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

    private void setupCard(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CardEntity>()
            .HasKey(c => c.CardID);

        modelBuilder.Entity<CardEntity>()
            .HasOne(c => c.Player)
            .WithMany(p => p.Cards)
            .HasForeignKey(c => c.PlayerID)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void setupBoardCard(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BoardCardEntity>()
            .HasKey(bc => bc.BoardCardID);

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