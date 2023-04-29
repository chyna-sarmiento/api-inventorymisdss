using api_inventorymisdss.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace api_inventorymisdss.Repository.EntityConfiguration
{
    public class OutgoingConfiguration : IEntityTypeConfiguration<Outgoing>
    {
        public void Configure(EntityTypeBuilder<Outgoing> builder)
        {
            builder.ToTable("Outgoing", "Inventory");
            builder.HasKey(o => o.Id);

            builder.HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}