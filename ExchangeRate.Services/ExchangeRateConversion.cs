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
        double BuyTransfers(double amount, CurrencyCodes currencyCode);
        double BuyCheques(double amount, CurrencyCodes currencyCode);
        double BuyNotes(double amount, CurrencyCodes currencyCode);
        double SellCheques(double amount, CurrencyCodes currencyCode);
        double SellNotes(double amount, CurrencyCodes currencyCode);
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

        public double BuyTransfers(double amount, CurrencyCodes currencyCode)
        {
            try
            {
                var exchangerate = _exchangeRateContext
                    .Set<ExchangeRates>()
                    .GroupBy(x => new { x.CurrencyCode, x.DateCreated, x.BuyTransfers, x.Multiplier })
                    .Where(e => e.Key.CurrencyCode == currencyCode.ToString())
                    .OrderByDescending(e => e.Key.DateCreated.Date)
                    .Select(e => new { e.Key.BuyTransfers, e.Key.Multiplier })
                    .First();

                if (exchangerate == null)
                {
                    _log.LogError("Unable to get latest exchangerate for: {0} ", currencyCode.ToString());
                    throw new ArgumentException("Could not get the exchangerate for this currency");
                }

                return convert(exchangerate.Multiplier, amount, Convert.ToDouble(exchangerate.BuyTransfers));
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
                throw;
            }

        }

        public double BuyCheques(double amount, CurrencyCodes currencyCode)
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

                return convert(exchangerate.Multiplier, amount, Convert.ToDouble(exchangerate.BuyCheques));
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
                throw;
            }


        }

        public double BuyNotes(double amount, CurrencyCodes currencyCode)
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

                return convert(exchangerate.Multiplier, amount, Convert.ToDouble(exchangerate.BuyNotes));
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
                throw;
            }

        }

        public double SellCheques(double amount, CurrencyCodes currencyCode)
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

                return convert(exchangerate.Multiplier, amount, Convert.ToDouble(exchangerate.SellCheques));
            }
            catch(Exception ex)
            {
                _log.LogError(ex.Message, ex);
                throw;
            }


        }

        public double SellNotes(double amount, CurrencyCodes currencyCode)
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

                return convert(exchangerate.Multiplier, amount, Convert.ToDouble(exchangerate.SellNotes));
            }
            catch(Exception ex)
            {
                _log.LogError(ex.Message, ex);
                throw;
            }

        }

        private double convert(string opperator, double amount, double exchangerate)
        {

            switch (opperator)
            {
                case "/":
                    return RoundUp((amount / exchangerate), 2);
                case "*":
                   return RoundUp((amount * exchangerate), 2);
                default:
                    return 0.00;

            }
        }

        public double RoundUp(double input, int places)
        {
            return Math.Round(input, places, MidpointRounding.AwayFromZero);
        }


    }
}
