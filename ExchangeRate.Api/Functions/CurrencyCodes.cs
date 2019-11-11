using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using ExchangeRate.Api.Models;

namespace ExchangeRate.Api.Functions
{
    public class CurrencyCodes: ControllerBase
    {
        [FunctionName("CurrencyCodes")]
        public IActionResult Run( [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {

            var result = new List<Lookup>();

            foreach (var transactiontype in Enum.GetValues(typeof(Services.Enums.CurrencyCodes)))
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
