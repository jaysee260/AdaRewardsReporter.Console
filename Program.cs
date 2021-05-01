using System.Threading.Tasks;
using NDesk.Options;

namespace ADARewardsReporter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string stakeAddress = null;

            var options = new OptionSet
            {
                { "stakeAddress=", v => stakeAddress = v }
            };
            options.Parse(args);

            if (stakeAddress == null)
            {
                System.Console.WriteLine("A stake address must be provided. Aborting.");
                return;
            }
            
            var blockchainClient = new BlockfrostClient(
                ConfigManager.GetConfigurationvalue("MainnetBaseUrl"),
                ConfigManager.GetConfigurationvalue("AuthenticationHeaderKey"),
                ConfigManager.GetConfigurationvalue("ApiKey")
            );

            var rewardsReporter = new RewardsReporter(blockchainClient);
            await rewardsReporter.RunAsync(stakeAddress);
        }
    }
}
