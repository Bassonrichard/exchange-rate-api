using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ExchangeRate.Services;

namespace ExchangeRate.Api
{
    public  class ExchangeRates : ControllerBase
    {
        private readonly IScraper _scraper;

        private readonly ILogger<ExchangeRates> _logger;

        public ExchangeRates(IScraper scraper, ILogger<ExchangeRates> logger)
        {
            _scraper = scraper;
            _logger = logger;
        }

        [FunctionName("ExchangeRates")]
        public  async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            var data = await _scraper.GetExchnageRates();

            return Ok(data);
        }
    }
}
