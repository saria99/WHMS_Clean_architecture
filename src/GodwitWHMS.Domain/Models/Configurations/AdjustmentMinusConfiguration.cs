using GodwitWHMS.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GodwitWHMS.Domain.Models.Configurations
{
    public class AdjustmentMinusConfiguration : _BaseConfiguration<AdjustmentMinus>
    {
        public override void Configure(EntityTypeBuilder<AdjustmentMinus> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.Number).HasMaxLength(100);
            builder.Property(c => c.Description).HasMaxLength(255);
        }
    }
}
