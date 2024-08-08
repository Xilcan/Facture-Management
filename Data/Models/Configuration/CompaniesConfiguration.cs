using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models.Configuration;

internal class CompaniesConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(c => c.Factures)
            .WithOne(f => f.Company)
            .HasForeignKey(f => f.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.UserFactures)
                .WithOne(f => f.UserCompany)
                .HasForeignKey(e => e.UserCompanyId)
                .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Address)
            .WithOne(f => f.Company)
            .HasForeignKey<CompanyAddress>(a => a.CompanyId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
