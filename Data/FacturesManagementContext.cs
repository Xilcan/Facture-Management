using Data.Models;
using Data.Models.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class FacturesManagementContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<CompanyAddress> CompanyAddresses { get; set; }

    public DbSet<Company> Companies { get; set; }

    public DbSet<FactureDetail> FactureDetails { get; set; }

    public DbSet<Facture> Factures { get; set; }

    public DbSet<Payment> Payments { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<ProductCategory> ProductCategories { get; set; }

    public DbSet<PdfFile> PdfFiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\\localDB;Database=FacturesManagement;Trusted_Connection=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CompaniesConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyAddressesConfiguration());
        modelBuilder.ApplyConfiguration(new FactureDetailConfiguration());
        modelBuilder.ApplyConfiguration(new FacturesConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ProductsConfiguration());
        modelBuilder.ApplyConfiguration(new PdfFileConfiguration());
    }
}
