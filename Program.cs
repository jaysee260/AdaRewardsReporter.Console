using System;
using System.Threading.Tasks;
using ADARewardsReporter.Models;
using ADARewardsReporter.Utils;
using NDesk.Options;

namespace ADARewardsReporter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string stakeAddress = null;
            OrderBy orderBy = OrderBy.Desc;

            var options = new OptionSet
            {
                { "stakeAddress=", v => stakeAddress = v },
                { "orderBy=", v => Enum.TryParse<OrderBy>(v, true, out orderBy) }
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
            await rewardsReporter.RunAsync(stakeAddress, orderBy);
        }
    }
}
