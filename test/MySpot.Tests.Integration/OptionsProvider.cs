using Microsoft.Extensions.Configuration;
using MySpot.Infrastructure;

namespace MySpot.Tests.Integration
{
    public sealed class OptionsProvider
    {
        private readonly IConfigurationRoot _configuration;

        public OptionsProvider()
        {
            _configuration = GetConfigurationRoot();
        }

        public T Get<T>(string sectioName) where T : class, new()
            => _configuration.GetOptions<T>(sectioName);

        private static IConfigurationRoot GetConfigurationRoot()
            => new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json", true)
            .AddEnvironmentVariables()
            .Build();
    }
}
