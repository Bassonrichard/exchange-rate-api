using System;

namespace ExchangeRate.Common
{
    public interface ISettings {
        string AbsaExchnageRateUrl { get; }
        string TableStorageAccount { get; }
        string ServiceBus { get; }
    }

    
    public class Settings : ISettings
    {
        public string AbsaExchnageRateUrl { get; } = Environment.GetEnvironmentVariable("AbsaExchnageRateUrl");
        public string TableStorageAccount { get; } = Environment.GetEnvironmentVariable("TableStorageAccount");
        public string ServiceBus { get; } = Environment.GetEnvironmentVariable("ServiceBus");
    }
}
