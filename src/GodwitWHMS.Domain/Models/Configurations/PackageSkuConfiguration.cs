using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using GodwitWHMS.Domain.Models.Entities;

namespace GodwitWHMS.Domain.Models.Configurations;

public class PackageSkuConfiguration : _BaseConfiguration<PackageSku>
{
    public void Configure(EntityTypeBuilder<PackageSku> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Code).IsRequired().HasMaxLength(100);
        builder.Property(e => e.ScannedCode).IsRequired().HasMaxLength(255);
    }
}
