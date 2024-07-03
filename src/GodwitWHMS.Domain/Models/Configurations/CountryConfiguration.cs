using GodwitWHMS.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GodwitWHMS.Domain.Models.Configurations
{
    public class CountryConfiguration : _BaseConfiguration<Country>
    {
        public override void Configure(EntityTypeBuilder<Country> builder)
        {
            base.Configure(builder);

            builder.HasKey(c => c.Id);
            builder.Property(c => c.CountryCode)
                .IsRequired()
                .HasMaxLength(4);
            builder.Property(c => c.CountryName)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
