using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models.Configuration;

internal class ProductsConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(e => e.TransactionType)
        .IsRequired()
        .HasConversion<string>();

        builder.Property(fd => fd.Price)
            .HasColumnType("decimal(18, 2)")
            .IsRequired();

        builder.Property(fd => fd.Vat)
           .HasColumnType("decimal(18, 2)")
           .IsRequired();

        builder.HasMany(p => p.FactureDetails)
            .WithOne(fd => fd.Products)
            .HasForeignKey(fd => fd.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Category)
            .WithMany(pc => pc.Products)
            .HasForeignKey(p => p.CategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
