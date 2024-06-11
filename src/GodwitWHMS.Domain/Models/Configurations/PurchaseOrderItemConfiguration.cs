using GodwitWHMS.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GodwitWHMS.Domain.Models.Configurations
{
    public class PurchaseOrderItemConfiguration : _BaseConfiguration<PurchaseOrderItem>
    {
        public override void Configure(EntityTypeBuilder<PurchaseOrderItem> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.PurchaseOrderId).IsRequired();
            builder.Property(c => c.ProductId).IsRequired();
            builder.Property(c => c.Summary).HasMaxLength(100);
        }
    }
}
