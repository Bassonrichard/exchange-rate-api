using ExchangeRate.Services.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Services
{
    public interface IExchangeRateConversion
    {
        Task<double> BuyTransfers(double amount, CurrencyCodes currencyCode);
        Task<double> BuyCheques(double amount, CurrencyCodes currencyCode);
        Task<double> BuyNotes(double amount, CurrencyCodes currencyCode);
        Task<double> SellCheques(double amount, CurrencyCodes currencyCode);
        Task<double> SellNotes(double amount, CurrencyCodes currencyCode);
    }

    public class ExchangeRateConversion : IExchangeRateConversion
    {
        private readonly ILogger<ExchangeRateConversion> _log;
        private readonly IAzureTableStorage _azureTableStorage;
        public ExchangeRateConversion(ILogger<ExchangeRateConversion> log, IAzureTableStorage azureTableStorage)
        {
            _log = log;
            _azureTableStorage = azureTableStorage;
        }

        public async Task<double> BuyTransfers(double amount, CurrencyCodes currencyCode)
        {
            try
            {
                string partition = currencyCode.ToString();
                string row = DateTime.Now.ToString("yyyy-MM-dd");

                var exchange = await _azureTableStorage.RetrieveExchangeRateUsingPointQueryAsync(partition,row);
                var exchangerate = new { Multiplier = exchange.Multiplier, BuyTransfers = exchange.BuyTransfers};

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

        public async Task<double> BuyCheques(double amount, CurrencyCodes currencyCode)
        {
            try
            {
                string partition = currencyCode.ToString();
                string row = DateTime.Now.ToString("yyyy-MM-dd");

                var exchange = await _azureTableStorage.RetrieveExchangeRateUsingPointQueryAsync(partition, row);
                var exchangerate = new { Multiplier = exchange.Multiplier, BuyCheques = exchange.BuyCheques };
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

        public async Task<double> BuyNotes(double amount, CurrencyCodes currencyCode)
        {
            try
            {
                string partition = currencyCode.ToString();
                string row = DateTime.Now.ToString("yyyy-MM-dd");

                var exchange = await _azureTableStorage.RetrieveExchangeRateUsingPointQueryAsync(partition, row);
                var exchangerate = new { Multiplier = exchange.Multiplier, BuyNotes = exchange.BuyNotes };
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

        public async Task<double> SellCheques(double amount, CurrencyCodes currencyCode)
        {
            try
            {
                string partition = currencyCode.ToString();
                string row = DateTime.Now.ToString("yyyy-MM-dd");

                var exchange = await _azureTableStorage.RetrieveExchangeRateUsingPointQueryAsync(partition, row);
                var exchangerate = new { Multiplier = exchange.Multiplier, SellCheques = exchange.SellCheques };
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

        public async Task<double> SellNotes(double amount, CurrencyCodes currencyCode)
        {
            try
            {
                string partition = currencyCode.ToString();
                string row = DateTime.Now.ToString("yyyy-MM-dd");

                var exchange = await _azureTableStorage.RetrieveExchangeRateUsingPointQueryAsync(partition, row);
                var exchangerate = new { Multiplier = exchange.Multiplier, SellNotes = exchange.SellNotes};
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

        private  double convert(string opperator, double amount, double exchangerate)
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
