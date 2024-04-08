using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Configuration
{
    internal class FactureDetailConfiguration : IEntityTypeConfiguration<FactureDetail>
    {
        public void Configure(EntityTypeBuilder<FactureDetail> builder)
        {
           builder.HasKey(x => x.Id);

            builder.Property(fd => fd.UnitPrice)
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            builder.HasOne(fd => fd.Factures)
                .WithMany(f => f.FactureDetails)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(fd => fd.Products)
                .WithMany(p => p.FactureDetails)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
