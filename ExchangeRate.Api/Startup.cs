using ExchangeRate.Api;
using ExchangeRate.Common;
using ExchangeRate.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.ServiceBus;

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
            builder.Services.AddTransient<IServiceBus, ServiceBus>();

            builder.Services.AddHttpClient<Scraper>();
        }
    }
}
