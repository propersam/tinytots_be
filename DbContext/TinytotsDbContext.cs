using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Tinytots.Models;

namespace Tinytots.DbContext;

public class TinytotsDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public TinytotsDbContext(DbContextOptions<TinytotsDbContext> options) : base(options)
    {
    }

    // Define tables
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<Invoice>  Invoices { get; set; }
    
}
