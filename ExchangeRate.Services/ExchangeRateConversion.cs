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
            var exchangerate = _exchangeRateContext
                 .Set<ExchangeRates>()
                 .GroupBy(x => new { x.CurrencyCode, x.DateCreated, x.BuyTransfers })
                 .Where(e => e.Key.CurrencyCode == currencyCode.ToString())
                 .OrderByDescending(e => e.Key.DateCreated.Date)
                 .Select(e => e.Key.BuyTransfers)
                 .First();

            if (!exchangerate.HasValue)
            {
                _log.LogError("Unable to get latest exchangerate for: {0} ", currencyCode.ToString());
            }

           return amount * (decimal)exchangerate;
        }

        public decimal BuyCheques(decimal amount, CurrencyCodes currencyCode)
        {
            var exchangerate = _exchangeRateContext
            .Set<ExchangeRates>()
            .GroupBy(x => new { x.CurrencyCode, x.DateCreated, x.BuyCheques })
            .Where(e => e.Key.CurrencyCode == currencyCode.ToString())
            .OrderByDescending(e => e.Key.DateCreated.Date)
            .Select(e => e.Key.BuyCheques)
            .First();

            if (!exchangerate.HasValue)
            {
                _log.LogError("Unable to get latest exchangerate for: {0} ", currencyCode.ToString());
            }

            return amount * (decimal)exchangerate;
        }

        public decimal BuyNotes(decimal amount, CurrencyCodes currencyCode)
        {
            var exchangerate = _exchangeRateContext
            .Set<ExchangeRates>()
            .GroupBy(x => new { x.CurrencyCode, x.DateCreated, x.BuyNotes })
            .Where(e => e.Key.CurrencyCode == currencyCode.ToString())
            .OrderByDescending(e => e.Key.DateCreated.Date)
            .Select(e => e.Key.BuyNotes)
            .First();

            if (!exchangerate.HasValue)
            {
                _log.LogError("Unable to get latest exchangerate for: {0} ", currencyCode.ToString());
            }

            return amount * (decimal)exchangerate;
        }

        public decimal SellCheques(decimal amount, CurrencyCodes currencyCode)
        {
            var exchangerate = _exchangeRateContext
            .Set<ExchangeRates>()
            .GroupBy(x => new { x.CurrencyCode, x.DateCreated, x.SellCheques })
            .Where(e => e.Key.CurrencyCode == currencyCode.ToString())
            .OrderByDescending(e => e.Key.DateCreated.Date)
            .Select(e => e.Key.SellCheques)
            .First();

            if (!exchangerate.HasValue)
            {
                _log.LogError("Unable to get latest exchangerate for: {0} ", currencyCode.ToString());
            }

            return amount * (decimal)exchangerate;
        }

        public decimal SellNotes(decimal amount, CurrencyCodes currencyCode)
        {
            var exchangerate = _exchangeRateContext
            .Set<ExchangeRates>()
            .GroupBy(x => new { x.CurrencyCode, x.DateCreated, x.SellNotes })
            .Where(e => e.Key.CurrencyCode == currencyCode.ToString())
            .OrderByDescending(e => e.Key.DateCreated.Date)
            .Select(e => e.Key.SellNotes)
            .First();

            if (!exchangerate.HasValue)
            {
                _log.LogError("Unable to get latest exchangerate for: {0} ", currencyCode.ToString());
            }

            return amount * (decimal)exchangerate;
        }


    }
}
