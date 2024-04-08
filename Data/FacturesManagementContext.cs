using Data.Models;
using Data.Models.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    internal class FacturesManagementContext : DbContext
    {
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<CompanyAddress> CompanyAddresses { get; set; }

        public DbSet<Companies> Companies { get; set; }

        public DbSet<Customers> Customers { get; set; }

        public DbSet<FactureDetail> FactureDetails { get; set; }

        public DbSet<Factures> Factures { get; set; }

        public DbSet<Payments> Payments { get; set; }

        public DbSet<Products> Products { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\localDB;Database=FacturesManagement;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompaniesConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyAddressesConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerAdressesConfiguration());
            modelBuilder.ApplyConfiguration(new CustomersConfiguration());
            modelBuilder.ApplyConfiguration(new FactureDetailConfiguration());
            modelBuilder.ApplyConfiguration(new FacturesConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductsConfiguration());
        }
    }
}
