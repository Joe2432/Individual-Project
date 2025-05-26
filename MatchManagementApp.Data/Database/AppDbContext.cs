using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<MatchEntity> Matches { get; set; }
    public DbSet<PointEntity> Points { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new MatchEntityConfiguration());
        modelBuilder.ApplyConfiguration(new PointEntityConfiguration()); // Add this when defined
    }
}
