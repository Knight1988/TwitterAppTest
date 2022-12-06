using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using TwitterApp.Models;

namespace TwitterApp.Repositories;

public class TwitterContext : DbContext
{
    public DbSet<TweetModel> Tweets { get; set; }
    
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        // migrate any database changes on startup (includes initial db creation)
        Database.Migrate();
    }
    
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