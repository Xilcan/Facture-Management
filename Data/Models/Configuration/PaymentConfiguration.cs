using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Configuration
{
    internal class PaymentConfiguration : IEntityTypeConfiguration<Payments>
    {
        public void Configure(EntityTypeBuilder<Payments> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(fd => fd.Amount)
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            builder.Property(x => x.PaymentDeadline)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(x => x.PaymantDate)
                .HasColumnType("datetime2");

            builder.HasOne(p => p.Facture)
                .WithMany(f => f.Payments)
                .HasForeignKey(p => p.FactureId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
