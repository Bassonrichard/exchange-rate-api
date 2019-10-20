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
using System.Collections.Generic;
using ExchangeRate.Services.Enums;

namespace ExchangeRate.Api.Functions
{
    public class TransactionTypes: ControllerBase
    {
        [FunctionName("TransactionTypes")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,ILogger log)
        {
            var result = new List<Lookup>();

            foreach (var transactiontype in Enum.GetValues(typeof(Services.Enums.TransactionTypes)))
            {
                result.Add(new Lookup()
                {
                    Key = Convert.ToInt32(transactiontype),
                    Value = transactiontype.ToString()
                });
            }

            return Ok(result);
        }
    }
}
