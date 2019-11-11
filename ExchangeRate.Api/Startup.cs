using ExchangeRate.Api;
using ExchangeRate.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
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
            builder.Services.AddTransient<IAzureTableStorage, AzureTableStorage>();
            builder.Services.AddTransient<ISettings, Settings>();
            builder.Services.AddTransient<IExchangeRateConversion, ExchangeRateConversion>();
            builder.Services.AddTransient<IScraper, Scraper>();


            builder.Services.AddHttpClient<Scraper>();
        }
    }
}
