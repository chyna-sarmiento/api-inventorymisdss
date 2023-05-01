using api_inventorymisdss.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace api_inventorymisdss.Repository.EntityConfiguration
{
    public class IncomingConfiguration : IEntityTypeConfiguration<Incoming>
    {
        public void Configure(EntityTypeBuilder<Incoming> builder)
        {
            builder.ToTable("Incoming", "Inventory");
            builder.HasKey(i => i.Id);

            builder.HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(i => i.IncomingProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}