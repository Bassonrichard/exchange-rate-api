using ExchangeRate.Common;
using ExchangeRate.Services.Models;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Services
{
    public interface IAzureTableStorage
    {
        CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString);
        CloudTable GetTable(string tableName);
        Task<ExchangeRateModel> InsertOrMergeExchangeRateAsync(ExchangeRateModel entity);
        Task<ExchangeRateModel> RetrieveExchangeRateUsingPointQueryAsync(string partitionKey, string rowKey);
    }
    public class AzureTableStorage : IAzureTableStorage
    {
        private readonly ILogger<AzureTableStorage> _logger;
        private readonly ISettings _settings;
        public AzureTableStorage(ILogger<AzureTableStorage> logger, ISettings settings)
        {
            _logger = logger;
            _settings = settings;
        }
        public CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                Console.ReadLine();
                throw;
            }

            return storageAccount;
        }

        public CloudTable GetTable(string tableName)
        {
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(_settings.TableStorageAccount);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference(tableName);

            return table;
        }

        public async Task<ExchangeRateModel> InsertOrMergeExchangeRateAsync(ExchangeRateModel entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("ExchangeRates");
            }
            try
            {
                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);

                var table = GetTable("ExchangeRates");

                TableResult result = await table.ExecuteAsync(insertOrMergeOperation);

                var insertedCustomer = result.Result as ExchangeRateModel;


                return insertedCustomer;
            }
            catch (StorageException ex)
            {
                _logger.LogError(ex.Message);
                 throw ex;
            }
        }

        public async Task<ExchangeRateModel> RetrieveExchangeRateUsingPointQueryAsync(string partitionKey, string rowKey)
        {
            try
            {
                TableOperation retrieveOperation = TableOperation.Retrieve<ExchangeRateModel>(partitionKey, rowKey);

                var table = GetTable("ExchangeRates");

                TableResult result = await table.ExecuteAsync(retrieveOperation);

                ExchangeRateModel exchange = result.Result as ExchangeRateModel;

                return exchange;
            }
            catch (StorageException ex)
            {
                throw ex;
            }
        }
    }
}
