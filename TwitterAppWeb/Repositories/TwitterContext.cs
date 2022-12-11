using Microsoft.EntityFrameworkCore;
using TwitterAppWeb.Models;

namespace TwitterAppWeb.Repositories;

public class TwitterContext : DbContext
{
    public DbSet<TweetModel> Tweets { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=MyDatabase.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TweetModel>()
            .HasKey(t => t.Id);
    }
}