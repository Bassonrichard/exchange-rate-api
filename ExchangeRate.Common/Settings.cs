using System;

namespace ExchangeRate.Common
{
    public interface ISettings {
        string AbsaExchnageRateUrl { get; }
    }

    
    public class Settings : ISettings
    {
        public string AbsaExchnageRateUrl { get; } = Environment.GetEnvironmentVariable("AbsaExchnageRateUrl");
    }
}
