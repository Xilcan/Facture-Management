using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models.Configuration;

internal class CompanyAddressesConfiguration : IEntityTypeConfiguration<CompanyAddress>
{
    public void Configure(EntityTypeBuilder<CompanyAddress> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasOne(a => a.Company)
            .WithOne(a => a.Address)
            .HasForeignKey<CompanyAddress>(a => a.CompanyId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
