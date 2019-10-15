using ExchangeRate.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExchangeRate.EF
{
    public class ExchangeRateContextFactory : IDesignTimeDbContextFactory<ExchangeRateContext>
    {
        public ExchangeRateContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddEnvironmentVariables()
                            .Build();

            var connectionString = configuration.GetConnectionString("SqlServer");

            var optionsBuilder = new DbContextOptionsBuilder<ExchangeRateContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ExchangeRateContext(optionsBuilder.Options);
        }
    }
}
