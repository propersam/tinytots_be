using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tinytots.Models;

namespace Tinytots.DbContext
{

    public class TinytotsDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public TinytotsDbContext(DbContextOptions<TinytotsDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType.IsEnum)
                    {
                        // Create the right converter type for this enum
                        var converterType = typeof(EnumToStringConverter<>).MakeGenericType(property.ClrType);
                        var converter = (ValueConverter)Activator.CreateInstance(converterType, (ConverterMappingHints?)null)!;

                        property.SetValueConverter(converter);
                    }
                }
            }
            modelBuilder.Entity<Invoice>()
                .HasIndex(i => i.InvCode)
                .IsUnique();

            
        }
       
        // Define tables

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
    }
}