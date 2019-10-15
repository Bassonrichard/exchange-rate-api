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
using ExchangeRate.Api.Models;

namespace ExchangeRate.Api.Functions
{
    public class ExchnageValue: ControllerBase
    {

        private readonly IExchangeRateConversion _exchangeRateConversion;

        private readonly ILogger<ExchangeRates> _logger;

        public ExchnageValue(IExchangeRateConversion exchangeRateConversion, ILogger<ExchangeRates> logger)
        {
            _exchangeRateConversion = exchangeRateConversion;
            _logger = logger;
        }
        [FunctionName("ExchnageValue")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "Post", Route = null)] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ExchangeValue data = JsonConvert.DeserializeObject<ExchangeValue>(requestBody);
            decimal amount = 0.0m;
            try
            {
                switch (data.transactionTypes)
                {
                    case Services.Enums.TransactionTypes.BuyTransfer:
                        amount = _exchangeRateConversion.BuyTransfers(data.ammout, data.currencyCode);
                        break;
                    case Services.Enums.TransactionTypes.BuyCheques:
                        amount = _exchangeRateConversion.BuyCheques(data.ammout, data.currencyCode);
                        break;
                    case Services.Enums.TransactionTypes.SellCheques:
                        amount = _exchangeRateConversion.SellCheques(data.ammout, data.currencyCode);
                        break;
                    case Services.Enums.TransactionTypes.BuyNotes:
                        amount = _exchangeRateConversion.BuyNotes(data.ammout, data.currencyCode);
                        break;
                    case Services.Enums.TransactionTypes.SellNotes:
                        amount = _exchangeRateConversion.SellNotes(data.ammout, data.currencyCode);
                        break;
                }
                return Ok(amount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}
