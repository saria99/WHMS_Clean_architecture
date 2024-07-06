using GodwitWHMS.Domain.Models.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class CalculatedPriceConfiguration : _BaseConfiguration<CalculatedPrice>
{
    public override void Configure(EntityTypeBuilder<CalculatedPrice> builder)
    {
        base.Configure(builder);

        builder.Property(cp => cp.BasePriceId)
            .IsRequired();

        builder.Property(cp => cp.ServiceType)
            .IsRequired();

        builder.Property(cp => cp.PriceWithFuelSurcharge)
            .IsRequired();

        builder.Property(cp => cp.TotalPrice)
            .IsRequired();

        builder.Property(cp => cp.Weight)
            .IsRequired();

        builder.Property(cp => cp.IsCheapest);

        builder.HasOne(cp => cp.BasePrice)
            .WithMany()
            .HasForeignKey(cp => cp.BasePriceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
