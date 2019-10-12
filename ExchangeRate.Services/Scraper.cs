using ExchangeRate.EF;
using ExchangeRate.EF.Models;
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
        Task<List<ExchangeRates>> GetExchnageRates();
    }
    public class Scraper : IScraper
    {
        private ExchangeContext _exchangeContext;
        private readonly IHttpClientFactory _httpFactory;
        private readonly ILogger<Scraper> _log;

        public Scraper(IHttpClientFactory httpFactory, ILogger<Scraper> log, ExchangeContext exchangeContext)
        {
            _httpFactory = httpFactory;
            _log = log;
            _exchangeContext = exchangeContext;

        }

        public async Task<List<ExchangeRates>> GetExchnageRates()
        {
            var result = new List<ExchangeRates>();

            string url = "https://www.absa.co.za/indices/exchange-rates/";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var client = _httpFactory.CreateClient();

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
                result = data.Select(row => new ExchangeRates
                {
                    CurrencyCode = row[0],
                    Country = row[1],
                    Multiplier = row[2],
                    BuyTransfers = double.Parse(row[3], CultureInfo.InvariantCulture),
                    BuyCheques = double.Parse(row[4], CultureInfo.InvariantCulture),
                    BuyNotes = double.Parse(row[5], CultureInfo.InvariantCulture),
                    SellChecks = double.Parse(row[6], CultureInfo.InvariantCulture),
                    SellNotes = double.Parse(row[7], CultureInfo.InvariantCulture),

                }).ToList();

                _exchangeContext
                    .Set<ExchangeRates>()
                    .AddRange(result);

                _exchangeContext.SaveChanges();
            }

            return result;
        }
    }
}
