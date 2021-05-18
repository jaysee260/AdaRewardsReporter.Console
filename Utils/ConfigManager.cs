using Microsoft.Extensions.Configuration;

namespace ADARewardsReporter.Utils
{
    public static class ConfigManager
    {
        private static readonly IConfiguration _config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.Local.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        public static string GetConfigurationvalue(string key)
        {
            return _config[key];
        }
    }
}