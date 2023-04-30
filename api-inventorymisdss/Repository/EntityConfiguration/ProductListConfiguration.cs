using api_inventorymisdss.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_inventorymisdss.Repository.EntityConfiguration
{
    public class ProductListConfiguration : IEntityTypeConfiguration<ProductList>
    {
        public void Configure(EntityTypeBuilder<ProductList> builder)
        {
            builder.ToTable("ProductList", "InventoryList");
            builder.HasKey(p => p.ProductId);

            builder.HasOne(p => p.Product)
                .WithOne(p => p.ProductList)
                .HasForeignKey<ProductList>(p => p.ProductId);
        }
    }
}