using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using api_inventorymisdss.Domain;

namespace api_inventorymisdss.Repository.EntityConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product", "Inventory");
            builder.HasKey(p => p.Id);
        }
    }
}