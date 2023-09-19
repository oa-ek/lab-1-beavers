using Core;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Contexts;

public class ApplicationContext : DbContext
{
    public DbSet<Developer> Developers { get; set; } = null!;
    public DbSet<Game> Games { get; set; } = null!;
    public DbSet<Store> Stores { get; set; } = null!;
    public DbSet<GameTag> GameTags { get; set; } = null!;
    public DbSet<Price> Prices { get; set; } = null!;
    public DbSet<Publisher> Publishers { get; set; } = null!;
    public DbSet<Tag> Tag { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserGameOwnership> UserGameOwnerships { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<Screenshot> Screenshots { get; set; } = null!;
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) // Configuration of database's context
    {
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

        modelBuilder.Entity<User>()
            .HasKey(g => g.UserId);

        modelBuilder.Entity<UserGameOwnership>()
            .HasKey(d => d.OwnershipId);

        modelBuilder.Entity<UserRole>()
            .HasKey(g => g.RoleId);

        modelBuilder.Entity<Screenshot>()
            .HasKey(g => g.ScreenshotId);

        modelBuilder.Entity<Developer>().Navigation(d => d.Games).AutoInclude();
    }

    private static void AddAutoIncludes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().Navigation(user => user.UserRole).AutoInclude();
        modelBuilder.Entity<Screenshot>().Navigation(screenshot => screenshot.Game).AutoInclude();
        modelBuilder.Entity<GameTag>().Navigation(screenshot => screenshot.Game).AutoInclude();
        modelBuilder.Entity<GameTag>().Navigation(screenshot => screenshot.Tag).AutoInclude();
    }
}