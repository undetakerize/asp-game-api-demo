using GameService.Domain.Entity.GameReviews;
using GameService.Domain.Entity.Games;
using GameService.Domain.Entity.Reviews;
using GameService.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameService.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
        base.OnConfiguring(optionsBuilder);
    }
    public DbSet<Game> Game { get; set; }
    public DbSet<Review> Review { get; set; }
    public DbSet<GameReview> GameReview { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        List<IdentityRole> roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Id = "1",
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "4d2483a6-9ba5-4c1d-8aef-7cb7d84a2185"
            },
            new IdentityRole
            {
                Id = "2",
                Name = "User",
                NormalizedName = "USER",
                ConcurrencyStamp = "4d2483a6-9ba5-4c1d-8aef-7cb7d84a2185"
            },
            new IdentityRole
            {
                Id = "3",
                Name = "Guest",
                NormalizedName = "GUEST",
                ConcurrencyStamp = "8c4d1013-fff6-4f21-a376-169f0abbb0ac"
            }
        };
        builder.Entity<IdentityRole>().HasData(roles);

        builder.Entity<GameReview>()
            .HasKey(gr => new { gr.GameId, gr.ReviewId });

        builder.Entity<GameReview>()
            .HasOne(gr => gr.Game)
            .WithMany(g => g.GameReviews)
            .HasForeignKey(gr => gr.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<GameReview>()
            .HasOne(gr => gr.Review)
            .WithMany(r => r.GameReviews)
            .HasForeignKey(gr => gr.ReviewId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
}