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

[assembly: WebJobsStartup(typeof(Startup))]
namespace ExchangeRate.Api
{
    public class Startup: IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services.AddTransient<IScraper, Scraper>();
            builder.Services.AddHttpClient<Scraper>();
            builder.Services.AddDbContext<ExchangeContext>();
        }
    }
}
