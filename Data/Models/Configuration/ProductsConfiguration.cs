using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Configuration
{
    internal class ProductsConfiguration : IEntityTypeConfiguration<Products>
    {
        public void Configure(EntityTypeBuilder<Products> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(e => e.TransactionType)
            .IsRequired()
            .HasConversion<string>();

            builder.Property(fd => fd.Price)
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
}
