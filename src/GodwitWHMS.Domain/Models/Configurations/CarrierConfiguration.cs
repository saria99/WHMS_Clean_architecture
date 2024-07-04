using GodwitWHMS.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GodwitWHMS.Domain.Models.Configurations
{
    public class CarrierConfiguration : _BaseConfiguration<Carrier>
    {
        public override void Configure(EntityTypeBuilder<Carrier> builder)
        {
            base.Configure(builder);

            builder.HasKey(c => c.Id);
            builder.Property(c => c.CarrierName)
                .IsRequired();
        }
    }
}
