using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RVezy.Infra.Infra.Entities;

namespace RVezy.Infra.Infra.Mappings
{
    public class CalendarMappings : IEntityTypeConfiguration<Calendar>
    {
        public void Configure(EntityTypeBuilder<Calendar> builder)
        {
            builder.ToTable(nameof(Calendar));
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.ListingId);
            builder.Property(c => c.Date);
            builder.Property(c => c.Available);
            builder.Property(c => c.Price);


            builder.HasOne(i => i.Listing)
                   .WithMany(e => e.Calendars)
                   .HasForeignKey(t => t.ListingId)
                   .HasConstraintName($"FK_{nameof(Calendar)}_{nameof(Calendar.ListingId)}");

            builder.HasIndex(i => i.ListingId, $"IX_{nameof(Calendar)}_{nameof(Calendar.ListingId)}");

        }
    }
}
