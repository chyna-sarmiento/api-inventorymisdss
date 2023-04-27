using api_inventorymisdss.Domain;
using Microsoft.EntityFrameworkCore;

namespace api_inventorymisdss.Repository
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options){}

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Incoming> Incomings { get; set; }
        public virtual DbSet<Outgoing> Outgoings { get; set; }
    }
}