using ExchangeRate.EF.Models;
using ExchangeRate.Services.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExchangeRate.Services
{
    public interface IExchangeRateConversion
    {
        decimal BuyTransfers(decimal amount, CurrencyCodes currencyCode);
        decimal BuyCheques(decimal amount, CurrencyCodes currencyCode);
        decimal BuyNotes(decimal amount, CurrencyCodes currencyCode);
        decimal SellCheques(decimal amount, CurrencyCodes currencyCode);
        decimal SellNotes(decimal amount, CurrencyCodes currencyCode);
    }

    public class ExchangeRateConversion : IExchangeRateConversion
    {
        private ExchangeRateContext _exchangeRateContext;
        private readonly ILogger<ExchangeRateConversion> _log;
        public ExchangeRateConversion(ExchangeRateContext exchangeRateContext, ILogger<ExchangeRateConversion> log)
        {
            _exchangeRateContext = exchangeRateContext;
            _log = log;
        }

        public decimal BuyTransfers(decimal amount, CurrencyCodes currencyCode)
        {
            try
            {
                var exchangerate = _exchangeRateContext
             .Set<ExchangeRates>()
             .GroupBy(x => new { x.CurrencyCode, x.DateCreated, x.BuyTransfers, x.Multiplier })
             .Where(e => e.Key.CurrencyCode == currencyCode.ToString())
             .OrderByDescending(e => e.Key.DateCreated.Date)
             .Select( e => new { e.Key.BuyTransfers, e.Key.Multiplier })
             .First();

                if (exchangerate == null)
                {
                    _log.LogError("Unable to get latest exchangerate for: {0} ", currencyCode.ToString());
                    throw new ArgumentException("Could not get the exchangerate for this currency");
                }

                return convert(exchangerate.Multiplier, amount, (decimal)exchangerate.BuyTransfers);
            }
            catch
            {
                throw;
            }

        }

        public decimal BuyCheques(decimal amount, CurrencyCodes currencyCode)
        {
            try
            {
                var exchangerate = _exchangeRateContext
                 .Set<ExchangeRates>()
                 .GroupBy(x => new { x.CurrencyCode, x.DateCreated, x.BuyCheques, x.Multiplier })
                 .Where(e => e.Key.CurrencyCode == currencyCode.ToString())
                 .OrderByDescending(e => e.Key.DateCreated.Date)
                 .Select(e => new { e.Key.BuyCheques, e.Key.Multiplier })
                 .First();

                if (exchangerate == null)
                {
                    _log.LogError("Unable to get latest exchangerate for: {0} ", currencyCode.ToString());
                    throw new ArgumentException("Could not get the exchangerate for this currency");
                }

                return convert(exchangerate.Multiplier, amount, (decimal)exchangerate.BuyCheques);
            }
            catch
            {
                throw;
            }

        }

        public decimal BuyNotes(decimal amount, CurrencyCodes currencyCode)
        {
            try
            {
                var exchangerate = _exchangeRateContext
                             .Set<ExchangeRates>()
                             .GroupBy(x => new { x.CurrencyCode, x.DateCreated, x.BuyNotes, x.Multiplier })
                             .Where(e => e.Key.CurrencyCode == currencyCode.ToString())
                             .OrderByDescending(e => e.Key.DateCreated.Date)
                             .Select(e => new { e.Key.BuyNotes, e.Key.Multiplier })
                             .First();

                if (exchangerate == null)
                {
                    _log.LogError("Unable to get latest exchangerate for: {0} ", currencyCode.ToString());
                    throw new ArgumentException("Could not get the exchangerate for this currency");
                }

                return convert(exchangerate.Multiplier, amount, (decimal)exchangerate.BuyNotes);
            }
            catch
            {
                throw;
            }

        }

        public decimal SellCheques(decimal amount, CurrencyCodes currencyCode)
        {
            try
            {

                var exchangerate = _exchangeRateContext
                .Set<ExchangeRates>()
                .GroupBy(x => new { x.CurrencyCode, x.DateCreated, x.SellCheques, x.Multiplier })
                .Where(e => e.Key.CurrencyCode == currencyCode.ToString())
                .OrderByDescending(e => e.Key.DateCreated.Date)
                .Select(e => new { e.Key.SellCheques, e.Key.Multiplier })
                .First();

                if (exchangerate == null)
                {
                    _log.LogError("Unable to get latest exchangerate for: {0} ", currencyCode.ToString());
                    throw new ArgumentException("Could not get the exchangerate for this currency");
                }

                return convert(exchangerate.Multiplier, amount, (decimal)exchangerate.SellCheques);
            }
            catch
            {
                throw;
            }

        }

        public decimal SellNotes(decimal amount, CurrencyCodes currencyCode)
        {
            try
            {
                var exchangerate = _exchangeRateContext
                  .Set<ExchangeRates>()
                  .GroupBy(x => new { x.CurrencyCode, x.DateCreated, x.SellNotes, x.Multiplier })
                  .Where(e => e.Key.CurrencyCode == currencyCode.ToString())
                  .OrderByDescending(e => e.Key.DateCreated.Date)
                  .Select(e => new { e.Key.SellNotes, e.Key.Multiplier })
                  .FirstOrDefault();

                if (exchangerate == null)
                {
                    _log.LogError("Unable to get latest exchangerate for: {0} ", currencyCode.ToString());
                    throw new ArgumentException("Could not get the exchangerate for this currency");
                }

                return convert(exchangerate.Multiplier, amount, (decimal)exchangerate.SellNotes);
            }
            catch
            {
                throw;
            }

        }


        private decimal convert(string opperator,decimal amount, decimal exchangerate)
        {

            switch (opperator)
            {
                case "/":
                    return Math.Ceiling(amount / exchangerate);
                case "*":
                   return Math.Ceiling(amount * exchangerate);
                default: 
                    return 0.0m;

            }
        }


    }
}
