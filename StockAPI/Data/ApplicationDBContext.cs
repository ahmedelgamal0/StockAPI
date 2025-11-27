// Import the main library for Entity Framework Core.
// This gives us access to "DbContext" and "DbSet".
using Microsoft.EntityFrameworkCore;

// Import the "Models" namespace from your own project.
// This allows this file to see and use your "Stock" and "Comment" classes.
using api.Models;

// These are standard C# libraries, not directly related to EF Core
// but commonly included in files.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

// This organizes your code. This file is located in the "Data" folder
// within the "api" project.
namespace api.Data;

// This is your main database context class.
// It "inherits" from (or gets all its powers from) the base DbContext class
// provided by Entity Framework.
public class ApplicationDBContext : IdentityDbContext<AppUser>
{
    // This is the class "constructor". It's a special method that runs
    // when a new "ApplicationDBContext" object is created.
    // It's used to pass in configuration.
    public ApplicationDBContext(DbContextOptions dbContextOptions)
    : base(dbContextOptions) // This "base" call passes the options *up* to the parent DbContext class.
    {
        // The options (dbContextOptions) tell the DbContext things like
        // *which* database to connect to (e.g., the "connection string").
        // This setup is usually done in another file (like Program.cs)
        // using "Dependency Injection".
    }

    // === Database Tables ===
    // These properties are the most important part for you.
    // Each "DbSet" represents a TABLE in your database.

    // This line tells Entity Framework:
    // 1. You want a table to store "Stock" objects.
    // 2. The table should be named "Stock" (based on the property name).
    // You will use this property to query for stocks (e.g., context.Stock.ToList()).
    public DbSet<Stock> Stocks { get; set; }

    // This line tells Entity Framework:
    // 1. You want a table to store "Comment" objects.
    // 2. The table should be named "Comment".
    public DbSet<Comment> Comments { get; set; }
    public DbSet<StockUser> StockUsers { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<StockUser>(x => x.HasKey(p => new { p.AppUserId, p.StockId }));
        builder.Entity<StockUser>()
        .HasOne(u => u.AppUser)
        .WithMany(u => u.StockUsers)
        .HasForeignKey(p => p.AppUserId);

        builder.Entity<StockUser>()
        .HasOne(u => u.Stock)
        .WithMany(u => u.StockUsers)
        .HasForeignKey(p => p.StockId);

        List<IdentityRole> roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Id = "6f0c5937-3988-4a62-951d-356f0242ac10",
                Name="Admin",
                NormalizedName="ADMIN"
            },
            new IdentityRole
            {
                Id = "7a1d6048-4099-5b73-842e-467g1353bd21",
                Name="User",
                NormalizedName="USER"
            }
        };
        builder.Entity<IdentityRole>().HasData(roles);
    }
}
