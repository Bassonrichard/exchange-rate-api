using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace ExchangeRate.EF.Models
{
    public partial class ExchangeRateContext : DbContext
    {

        public ExchangeRateContext(DbContextOptions<ExchangeRateContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ExchangeRates> ExchangeRates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddEnvironmentVariables()
                            .Build();

             

                var connectionString = configuration.GetConnectionString("SqlServer");
                SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString);
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<ExchangeRates>(entity =>
            {
                entity.Property(e => e.BuyCheques).HasColumnType("numeric(10, 4)");

                entity.Property(e => e.BuyNotes).HasColumnType("numeric(10, 4)");

                entity.Property(e => e.BuyTransfers).HasColumnType("numeric(10, 4)");

                entity.Property(e => e.SellCheques).HasColumnType("numeric(10, 4)");

                entity.Property(e => e.SellNotes).HasColumnType("numeric(10, 4)");
            });
        }
    }
}
