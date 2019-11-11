using System;
using System.Threading.Tasks;
using ExchangeRate.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ExchangeRate.Api.Functions
{
    public class ExchangeRatePoller
    {

        private readonly IScraper _scraper;

        private readonly ILogger<ExchangeRatePoller> _logger;
        public ExchangeRatePoller(IScraper scraper, ILogger<ExchangeRatePoller> logger)
        {
            _scraper = scraper;
            _logger = logger;
        }

        [FunctionName("ExchangeRatePoller")]
        public async Task Run([TimerTrigger("0 0 1 * * *")]TimerInfo myTimer)
        {
            try
            {
                await _scraper.GetExchnageRates();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
        }
    }
}
