namespace RaptorProject.Features.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Player> Player { get; set; }
    public DbSet<Game> Game { get; set; }
    public DbSet<Message> Message { get; set; }
    public DbSet<Board> Board { get; set; }
    public DbSet<Card> Card { get; set; }
    public DbSet<BoardCard> BoardCard { get; set; }

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
        modelBuilder.Entity<Player>()
            .HasKey(p => p.PlayerID);

        setupGame(modelBuilder);
        setupMessage(modelBuilder);
        setupBoard(modelBuilder);
        setupCard(modelBuilder);
        setupBoardCard(modelBuilder);
    }

    private void setupGame(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>()
            .HasKey(g => g.GameID);

        modelBuilder.Entity<Game>()
            .HasOne(g => g.PlayerOne)
            .WithMany(p => p.GamesAsPlayerOne)
            .HasForeignKey(g => g.PlayerOneID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Game>()
            .HasOne(g => g.PlayerTwo)
            .WithMany(p => p.GamesAsPlayerTwo)
            .HasForeignKey(g => g.PlayerTwoID)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void setupMessage(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>()
            .HasKey(m => m.MessageID);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Game)
            .WithMany(m => m.Messages)
            .HasForeignKey(m => m.GameID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Player)
            .WithMany(p => p.Messages)
            .HasForeignKey(m => m.PlayerID)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void setupBoard(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Board>()
            .HasKey(b => b.BoardID);

        modelBuilder.Entity<Board>()
            .HasOne(b => b.Game)
            .WithMany(g => g.Boards)
            .HasForeignKey(b => b.GameID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Board>()
            .HasOne(b => b.Player)
            .WithMany(p => p.Boards)
            .HasForeignKey(b => b.PlayerID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Board>()
            .HasOne(b => b.ChosenCard)
            .WithMany()
            .HasForeignKey(b => b.ChosenCardID)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void setupCard(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Card>()
            .HasKey(c => c.CardID);

        modelBuilder.Entity<Card>()
            .HasOne(c => c.Player)
            .WithMany(p => p.Cards)
            .HasForeignKey(c => c.PlayerID)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void setupBoardCard(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BoardCard>()
            .HasKey(bc => bc.BoardCardID);

        modelBuilder.Entity<BoardCard>()
            .HasOne(bc => bc.Card)
            .WithMany(c => c.BoardCards)
            .HasForeignKey(bc => bc.CardID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BoardCard>()
            .HasOne(bc => bc.Board)
            .WithMany(b => b.BoardCards)
            .HasForeignKey(bc => bc.BoardID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}