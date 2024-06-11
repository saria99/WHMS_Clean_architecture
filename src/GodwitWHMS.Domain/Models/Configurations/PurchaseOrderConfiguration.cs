using GodwitWHMS.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GodwitWHMS.Domain.Models.Configurations
{
    public class PurchaseOrderConfiguration : _BaseConfiguration<PurchaseOrder>
    {
        public override void Configure(EntityTypeBuilder<PurchaseOrder> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.VendorId).IsRequired();
            builder.Property(c => c.TaxId).IsRequired();
            builder.Property(c => c.Number).HasMaxLength(100);
            builder.Property(c => c.Description).HasMaxLength(255);
        }
    }
}
