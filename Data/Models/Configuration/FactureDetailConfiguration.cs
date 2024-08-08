using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models.Configuration;

internal class FactureDetailConfiguration : IEntityTypeConfiguration<FactureDetail>
{
    public void Configure(EntityTypeBuilder<FactureDetail> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(fd => fd.UnitPriceNetto)
            .HasColumnType("decimal(18, 2)")
            .IsRequired();

        builder.Property(fd => fd.Vat)
            .HasColumnType("decimal(18, 2)")
            .IsRequired();

        builder.HasOne(fd => fd.Factures)
            .WithMany(f => f.FactureDetails)
            .HasForeignKey(fd => fd.FactureId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(fd => fd.Products)
            .WithMany(p => p.FactureDetails)
            .HasForeignKey(fd => fd.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
