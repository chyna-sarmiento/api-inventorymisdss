using api_inventorymisdss.Domain;
using api_inventorymisdss.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace api_inventorymisdss.Repository
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Incoming> Incomings { get; set; }
        public virtual DbSet<Outgoing> Outgoings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                    .Property(p => p.Price)
                    .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Outgoing>()
                    .Property(o => o.ProductPrice)
                    .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Outgoing>()
                    .Property(o => o.TotalPrice)
                    .HasColumnType("decimal(18,2)");
        }
    }
}