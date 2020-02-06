using ExchangeRate.Common;
using ExchangeRate.Services.Models;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Services
{
    public interface IServiceBus
    {
        Task WriteToQueue(object obj);
    }
    public class ServiceBus: IServiceBus
    {
        private readonly ISettings _settings;
        private IQueueClient _queueClient;
        public ServiceBus(ISettings settings)
        {
            _settings = settings;
        }

        public async Task WriteToQueue(object obj)
        {
            try
            {
                _queueClient = new QueueClient(_settings.ServiceBus, "ExchangeRate");

                var json = JsonConvert.SerializeObject(obj);

                var message = new Message(Encoding.UTF8.GetBytes(json));

                await _queueClient.SendAsync(message);

                await _queueClient.CloseAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
