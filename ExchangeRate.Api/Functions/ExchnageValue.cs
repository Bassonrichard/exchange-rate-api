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
using ExchangeRate.Api.Models.Errors;
using ExchangeRate.Services.Enums;

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
            double amount = 0.00;

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                ExchangeValue data = JsonConvert.DeserializeObject<ExchangeValue>(requestBody);

                switch (data.transactionTypes)
                {
                    case Services.Enums.TransactionTypes.BuyTransfer:
                        amount = _exchangeRateConversion.BuyTransfers(data.amount, data.currencyCode);
                        break;
                    case Services.Enums.TransactionTypes.BuyCheques:
                        amount = _exchangeRateConversion.BuyCheques(data.amount, data.currencyCode);
                        break;
                    case Services.Enums.TransactionTypes.SellCheques:
                        amount = _exchangeRateConversion.SellCheques(data.amount, data.currencyCode);
                        break;
                    case Services.Enums.TransactionTypes.BuyNotes:
                        amount = _exchangeRateConversion.BuyNotes(data.amount, data.currencyCode);
                        break;
                    case Services.Enums.TransactionTypes.SellNotes:
                        amount = _exchangeRateConversion.SellNotes(data.amount, data.currencyCode);
                        break;
                    default: 
                        return NotFound(new NotFoundError($"There was no transaction type like '{data.transactionTypes}' found"));
                }

                var result = new ExchangeRateValueResp()
                {
                    initialValue = data.amount,
                    convertedValue = amount,
                    CurrencyCode = data.currencyCode.ToString()
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(new BadRequestError(ex.Message));
            }
        }
    }
}
