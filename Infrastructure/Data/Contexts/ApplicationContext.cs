using Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Contexts;

public class ApplicationContext : IdentityDbContext<User, UserRole, Guid>
{
    public DbSet<Developer> Developers { get; set; } = null!;
    public DbSet<Game> Games { get; set; } = null!;
    public DbSet<Store> Stores { get; set; } = null!;
    public DbSet<GameTag> GameTags { get; set; } = null!;
    public DbSet<Price> Prices { get; set; } = null!;
    public DbSet<Publisher> Publishers { get; set; } = null!;
    public DbSet<Tag> Tag { get; set; } = null!;
    public DbSet<UserGameOwnership> UserGameOwnerships { get; set; } = null!;
    public DbSet<Screenshot> Screenshots { get; set; } = null!;
    public DbSet<Vote> Votes { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) // Configuration of database's context
    {
        base.OnModelCreating(modelBuilder);
        
        AddKeyNavigations(modelBuilder);
        AddAutoIncludes(modelBuilder);
    }

    private static void AddKeyNavigations(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Developer>()
            .HasKey(d => d.DeveloperId);

        modelBuilder.Entity<Game>()
            .HasKey(g => g.GameId);

        modelBuilder.Entity<Store>()
            .HasKey(d => d.StoreId);

        modelBuilder.Entity<GameTag>()
            .HasKey(g => g.GameTagId);

        modelBuilder.Entity<Price>()
            .HasKey(d => d.PriceId);

        modelBuilder.Entity<Publisher>()
            .HasKey(g => g.PublisherId);

        modelBuilder.Entity<Tag>()
            .HasKey(d => d.TagId);

        modelBuilder.Entity<UserGameOwnership>()
            .HasKey(d => d.OwnershipId);

        modelBuilder.Entity<Screenshot>()
            .HasKey(g => g.ScreenshotId);

        modelBuilder.Entity<Comment>()
            .HasKey(g => g.CommentId);
        
        modelBuilder.Entity<Vote>()
            .HasKey(g => g.VoteId);
    }

    private static void AddAutoIncludes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Developer>().Navigation(d => d.Games).AutoInclude();
        modelBuilder.Entity<Screenshot>().Navigation(screenshot => screenshot.Game).AutoInclude();
        modelBuilder.Entity<GameTag>().Navigation(gameTag => gameTag.Game).AutoInclude();
        modelBuilder.Entity<GameTag>().Navigation(gameTag => gameTag.Tag).AutoInclude();
        modelBuilder.Entity<Game>().Navigation(game => game.Developer).AutoInclude();
        modelBuilder.Entity<Game>().Navigation(game => game.Publisher).AutoInclude();
        modelBuilder.Entity<Game>().Navigation(game => game.Screenshots).AutoInclude();
        modelBuilder.Entity<UserGameOwnership>().Navigation(ownership => ownership.Game).AutoInclude();
        modelBuilder.Entity<UserGameOwnership>().Navigation(ownership => ownership.User).AutoInclude();
        modelBuilder.Entity<GameTag>().Navigation(gameTag => gameTag.Game).AutoInclude();
        modelBuilder.Entity<GameTag>().Navigation(gameTag => gameTag.Tag).AutoInclude();
        modelBuilder.Entity<Price>().Navigation(price => price.Game).AutoInclude();
        modelBuilder.Entity<Price>().Navigation(price => price.Store).AutoInclude();
        modelBuilder.Entity<Vote>().Navigation(vote => vote.Comment).AutoInclude();
    }
}