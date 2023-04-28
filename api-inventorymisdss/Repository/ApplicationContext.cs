using api_inventorymisdss.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace api_inventorymisdss.Repository
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options){}

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Incoming> Incomings { get; set; }
        public virtual DbSet<Outgoing> Outgoings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Incoming>()
                .HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Outgoing>()
                .HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}