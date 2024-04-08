using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Configuration
{
    internal class FacturesConfiguration : IEntityTypeConfiguration<Factures>
    {
        public void Configure(EntityTypeBuilder<Factures> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreationDate)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(x => x.CreationDate)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(x => x.SaleDate)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(x => x.PaymentDate)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(e => e.LastModification)
            .IsRequired(false)
            .HasColumnType("datetime2");

            builder.HasOne(f => f.Customer)
                .WithMany(c => c.Factures)
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Company)
                .WithMany(c => c.Factures)
                .HasForeignKey(f => f.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(f => f.FactureDetails)
                .WithOne(fd => fd.Factures)
                .HasForeignKey(fd => fd.FactureId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(f => f.Payments)
                .WithOne(p => p.Facture)
                .HasForeignKey(p => p.FactureId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
