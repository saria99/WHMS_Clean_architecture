using GodwitWHMS.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GodwitWHMS.Domain.Models.Configurations
{
    public class BasePriceConfiguration : _BaseConfiguration<BasePrice>
    {
        public override void Configure(EntityTypeBuilder<BasePrice> builder)
        {
            base.Configure(builder);


            builder.Property(bp => bp.CarrierId)
                .IsRequired();

            builder.Property(bp => bp.OriginCountryId)
                .IsRequired();

            builder.Property(bp => bp.DestinationCountryId)
                .IsRequired();

            builder.Property(bp => bp.Weight)
                .IsRequired();

            builder.Property(bp => bp.Price)
                .IsRequired();

            builder.HasOne(bp => bp.Carrier)
                .WithMany()
                .HasForeignKey(bp => bp.CarrierId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(bp => bp.OriginCountry)
                .WithMany()
                .HasForeignKey(bp => bp.OriginCountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(bp => bp.DestinationCountry)
                .WithMany()
                .HasForeignKey(bp => bp.DestinationCountryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
