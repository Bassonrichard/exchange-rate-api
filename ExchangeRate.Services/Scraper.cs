using ExchangeRate.Common;
using ExchangeRate.Services.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRate.Services
{
    public interface IScraper
    {
        Task<List<ExchangeRateModel>> GetExchnageRates();
    }
    public class Scraper : IScraper
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly ILogger<Scraper> _log;
        private readonly ISettings _settings;
        private readonly IAzureTableStorage _azureTableStorage;
        private readonly IServiceBus _serviceBus;
        public Scraper(IHttpClientFactory httpFactory, ILogger<Scraper> log, ISettings settings, IAzureTableStorage azureTableStorage, IServiceBus serviceBus)
        {
            _httpFactory = httpFactory;
            _log = log;
            _settings = settings;
            _azureTableStorage = azureTableStorage;
            _serviceBus = serviceBus;
        }

        public async Task<List<ExchangeRateModel>> GetExchnageRates()
        {
            var result = new List<ExchangeRateModel>();

            string url = _settings.AbsaExchnageRateUrl;

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var client = _httpFactory.CreateClient();

            try
            {
                var response = await client.SendAsync(request);

                //Check success response
                if (response.IsSuccessStatusCode)
                {
                    var html = await response.Content.ReadAsStringAsync();

                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);

                    //Get table element from html
                    var tableNode = htmlDoc.DocumentNode.SelectNodes("//table")
                        .FirstOrDefault();

                    //Get table headers
                    var headerRow = tableNode.SelectNodes("//tr/th")
                            .Select(node => string.Intern(node.InnerText))
                            .ToArray();

                    //Get Table values
                    var data = tableNode.SelectNodes("//tbody/tr")
                                 .Select(rowNode => rowNode.SelectNodes("td")
                                         .Select(cellNode => cellNode.InnerText)
                                         .ToArray())
                                 .ToList();

                    //Parse data to object
                    result = data.Select(row => new ExchangeRateModel(row[0],DateTime.Now.ToString("yyyy-MM-dd"))
                    {
                        Country = row[1],
                        Multiplier = row[2],
                        BuyTransfers = double.Parse(row[3], CultureInfo.InvariantCulture),
                        BuyCheques = double.Parse(row[4], CultureInfo.InvariantCulture),
                        BuyNotes = double.Parse(row[5], CultureInfo.InvariantCulture),
                        SellCheques = double.Parse(row[6], CultureInfo.InvariantCulture),
                        SellNotes = double.Parse(row[7], CultureInfo.InvariantCulture)
                    }).ToList();
                }


                foreach (var exchange in result)
                {
                    await _serviceBus.WriteToQueue(exchange);
                    await _azureTableStorage.InsertOrMergeExchangeRateAsync(exchange);
                }

                return result.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
