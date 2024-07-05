using GodwitWHMS.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GodwitWHMS.Domain.Models.Configurations
{
    public class FuelSurchargeConfiguration : _BaseConfiguration<FuelSurcharge>
    {
        public override void Configure(EntityTypeBuilder<FuelSurcharge> builder)
        {
            base.Configure(builder);


            builder.Property(fs => fs.CarrierId).IsRequired();
            builder.Property(fs => fs.OriginCountryId).IsRequired();
            builder.Property(fs => fs.DestinationCountryId).IsRequired();
            builder.Property(fs => fs.EffectiveDate).IsRequired();
            builder.Property(fs => fs.FuelSurchargePercentage).IsRequired().HasColumnType("decimal(18,2)");

            builder.HasOne(fs => fs.Carrier)
                .WithMany()
                .HasForeignKey(fs => fs.CarrierId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(fs => fs.OriginCountry)
                .WithMany()
                .HasForeignKey(fs => fs.OriginCountryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(fs => fs.DestinationCountry)
                .WithMany()
                .HasForeignKey(fs => fs.DestinationCountryId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
