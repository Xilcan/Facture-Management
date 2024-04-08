using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Configuration
{
    internal class CompaniesConfiguration : IEntityTypeConfiguration<Companies>
    {
        public void Configure(EntityTypeBuilder<Companies> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(c => c.Factures)
                .WithOne(f => f.Company)
                .HasForeignKey(f => f.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Addresses)
                .WithOne(f => f.Company)
                .HasForeignKey(f => f.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
