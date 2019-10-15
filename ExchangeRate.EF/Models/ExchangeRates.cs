using System;
using System.Collections.Generic;

namespace ExchangeRate.EF.Models
{
    public partial class ExchangeRates
    {
        public int Id { get; set; }
        public string CurrencyCode { get; set; }
        public string Country { get; set; }
        public string Multiplier { get; set; }
        public decimal? BuyTransfers { get; set; }
        public decimal? BuyCheques { get; set; }
        public decimal? BuyNotes { get; set; }
        public decimal? SellCheques { get; set; }
        public decimal? SellNotes { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
