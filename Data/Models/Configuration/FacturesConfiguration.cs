using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models.Configuration;

internal class FacturesConfiguration : IEntityTypeConfiguration<Facture>
{
    public void Configure(EntityTypeBuilder<Facture> builder)
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

        builder.HasMany(f => f.PdfFiles)
            .WithOne(c => c.Facture)
            .HasForeignKey(f => f.FactureId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.UserCompany)
            .WithMany(c => c.UserFactures)
            .HasForeignKey(f => f.UserCompanyId)
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
