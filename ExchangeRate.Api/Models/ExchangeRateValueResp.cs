using ExchangeRate.Services.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRate.Api.Models
{
    public class ExchangeRateValueResp
    {
        public string CurrencyCode { get; set; }
        public decimal initialValue { get; set; }
        public decimal convertedValue { get; set; }
    }
}
