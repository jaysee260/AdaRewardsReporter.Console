using Microsoft.Extensions.Configuration;

namespace AdaRewardsReporter.Console.Utils
{
    public static class ConfigManager
    {
        private static readonly IConfiguration _config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.Local.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        public static IConfiguration GetConfiguration() => _config;
        
        public static string GetConfigurationValue(string key) => _config[key];
    }
}