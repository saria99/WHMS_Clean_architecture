using GodwitWHMS.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace GodwitWHMS.Domain.Models.Configurations
{
    public class CommissionConfiguration : IEntityTypeConfiguration<Commission>
    {
        public void Configure(EntityTypeBuilder<Commission> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.ServiceType).IsRequired().HasMaxLength(50);
            builder.Property(c => c.CommissionPercentage).IsRequired().HasColumnType("decimal(5, 2)");
            builder.Property(c => c.EffectiveDate).IsRequired();

            // Seed initial data
            builder.HasData(
                new Commission
                {
                    Id = 1,
                    RowGuid = Guid.NewGuid(),
                    ServiceType = "Economy",
                    CommissionPercentage = 15,
                    EffectiveDate = DateTime.Now
                },
                new Commission
                {
                    Id = 2,
                    RowGuid = Guid.NewGuid(),
                    ServiceType = "Express",
                    CommissionPercentage = 20,
                    EffectiveDate = DateTime.Now
                }
            );
        }
    }
}
