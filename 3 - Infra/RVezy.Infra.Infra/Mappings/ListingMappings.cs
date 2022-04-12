using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RVezy.Infra.Infra.Entities;

namespace RVezy.Infra.Infra.Mappings
{
    public class ListingMappings : IEntityTypeConfiguration<Listing>
    {
        public void Configure(EntityTypeBuilder<Listing> builder)
        {
            builder.ToTable(nameof(Listing));
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedNever();
            builder.Property(c => c.Description).HasMaxLength(1000);
            builder.Property(c => c.ListingUrl).HasMaxLength(1000);
            builder.Property(c => c.Name).HasMaxLength(200);
            builder.Property(c => c.PropertyType).HasMaxLength(50);
        }
    }
}
