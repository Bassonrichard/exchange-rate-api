using ExchangeRate.Services.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRate.Api.Models
{
    public class ExchangeRateValueResp
    {
        public string CurrencyCode { get; set; }
        public double initialValue { get; set; }
        public double convertedValue { get; set; }
    }
}
