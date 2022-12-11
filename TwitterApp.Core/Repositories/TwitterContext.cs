using Microsoft.EntityFrameworkCore;
using TwitterApp.Core.Models;

namespace TwitterApp.Core.Repositories;

public class TwitterContext : DbContext
{
    public DbSet<TweetModel> Tweets { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=TwitterDatabase.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TweetModel>()
            .HasKey(t => t.Id);
    }
}