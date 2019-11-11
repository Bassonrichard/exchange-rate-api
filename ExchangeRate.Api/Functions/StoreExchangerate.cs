using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ExchangeRate.Api.Models;
using ExchangeRate.Services;
using ExchangeRate.Services.Models;

namespace ExchangeRate.Api.Functions
{
    public class StoreExchangerate : ControllerBase
    {
        private readonly IAzureTableStorage _azureTableStorage;
        public StoreExchangerate(IAzureTableStorage azureTableStorage)
        {
            _azureTableStorage = azureTableStorage;
        }

        [FunctionName("StoreExchangerate")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<ExchangeRateView>(requestBody);

            var exchangeRate = new ExchangeRateModel(data.CurrencyCode, data.DateCreated.Date.ToString("yyyy-MM-dd"))
            {
                Country = data.Country,
                Multiplier = data.Multiplier,
                BuyCheques = data.BuyCheques,
                BuyNotes = data.BuyNotes,
                BuyTransfers = data.BuyTransfers,
                SellCheques = data.SellCheques,
                SellNotes = data.SellNotes,
            };

            var res = await _azureTableStorage.InsertOrMergeExchangeRateAsync(exchangeRate);


            return Ok(res);
        }
    }
}
