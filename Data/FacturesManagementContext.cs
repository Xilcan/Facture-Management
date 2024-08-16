using Data.Models;
using Data.Models.AuthenticationModels;
using Data.Models.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class FacturesManagementContext(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<CompanyAddress> CompanyAddresses { get; set; }

    public DbSet<Company> Companies { get; set; }

    public DbSet<FactureDetail> FactureDetails { get; set; }

    public DbSet<Facture> Factures { get; set; }

    public DbSet<Payment> Payments { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<ProductCategory> ProductCategories { get; set; }

    public DbSet<PdfFile> PdfFiles { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\\localDB;Database=FacturesManagement;Trusted_Connection=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new CompaniesConfiguration());
        builder.ApplyConfiguration(new CompanyAddressesConfiguration());
        builder.ApplyConfiguration(new FactureDetailConfiguration());
        builder.ApplyConfiguration(new FacturesConfiguration());
        builder.ApplyConfiguration(new PaymentConfiguration());
        builder.ApplyConfiguration(new ProductCategoryConfiguration());
        builder.ApplyConfiguration(new ProductsConfiguration());
        builder.ApplyConfiguration(new PdfFileConfiguration());
        builder.ApplyConfiguration(new RefreshTokenConfiguration());
    }
}
