using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;

namespace ExchangeRate.Services.Models
{
    public class ExchangeRateModel : TableEntity
    {
        public ExchangeRateModel()
        {

        }
        public ExchangeRateModel(string CurrencyCode, string Date)
        {
            PartitionKey = CurrencyCode;
            RowKey = Date;
        }
        public string Country { get; set; }
        public string Multiplier { get; set; }
        public double? BuyTransfers { get; set; }
        public double? BuyCheques { get; set; }
        public double? BuyNotes { get; set; }
        public double? SellCheques { get; set; }
        public double? SellNotes { get; set; }
    }
}
