using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RVezy.Infra.Infra.Entities;

namespace RVezy.Infra.Infra.Mappings
{
    public class ReviewMappings : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable(nameof(Review));
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.ListingId);
            builder.Property(c => c.Date);
            builder.Property(c => c.ReviewerId);
            builder.Property(c => c.ReviewerName);
            builder.Property(c => c.Comments);


            builder.HasOne(i => i.Listing)
                   .WithMany(e => e.Reviews)
                   .HasForeignKey(t => t.ListingId)
                   .HasConstraintName($"FK_{nameof(Review)}_{nameof(Review.ListingId)}");

            builder.HasIndex(i => i.ListingId, $"IX_{nameof(Review)}_{nameof(Review.ListingId)}");

        }
    }
}
