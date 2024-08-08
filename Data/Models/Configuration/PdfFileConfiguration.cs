using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models.Configuration;

internal class PdfFileConfiguration : IEntityTypeConfiguration<PdfFile>
{
    public void Configure(EntityTypeBuilder<PdfFile> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(c => c.Facture)
            .WithMany(f => f.PdfFiles)
            .HasForeignKey(f => f.FactureId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
