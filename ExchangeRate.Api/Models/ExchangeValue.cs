using ExchangeRate.Services.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRate.Api.Models
{
    public class ExchangeValue
    {
        public decimal ammout { get; set; }
        public CurrencyCodes currencyCode { get; set; }
        public TransactionTypes transactionTypes { get; set; }

    }
}
