using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRate.Api.Models
{
    public class ExchangeRateView
    {
        public string CurrencyCode { get; set; }
        public string Country { get; set; }
        public string Multiplier { get; set; }
        public double? BuyTransfers { get; set; }
        public double? BuyCheques { get; set; }
        public double? BuyNotes { get; set; }
        public double? SellCheques { get; set; }
        public double? SellNotes { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
