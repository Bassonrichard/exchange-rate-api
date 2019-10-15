using ExchangeRate.Api;
using ExchangeRate.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using ExchangeRate.EF;
using ExchangeRate.EF.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using ExchangeRate.Common;

[assembly: WebJobsStartup(typeof(Startup))]
namespace ExchangeRate.Api
{
    public class Startup: IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {

            IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddEnvironmentVariables()
                        .Build();

            var connectionString = configuration.GetConnectionString("SqlServer");

            builder.Services.AddTransient<ISettings, Settings>();
            builder.Services.AddTransient<IExchangeRateConversion, ExchangeRateConversion>();
            builder.Services.AddTransient<IScraper, Scraper>();
            builder.Services.AddHttpClient<Scraper>();
            builder.Services.AddDbContext<ExchangeRateContext>
                (options => options.UseSqlServer(connectionString));
        }
    }
}
