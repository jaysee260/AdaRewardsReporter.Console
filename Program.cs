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
            string regularAddress = null;
            OrderBy orderBy = OrderBy.Desc;
            bool exportToCsv = false;

            var options = new OptionSet
            {
                { "stakeAddress=", v => stakeAddress = v },
                { "regularAddress=", v => regularAddress = v },
                { "orderBy=", v => Enum.TryParse<OrderBy>(v, true, out orderBy) },
                { "exportToCsv", v => exportToCsv = true }
            };
            options.Parse(args);

            if (stakeAddress == null && regularAddress == null)
            {
                Console.WriteLine("A stake address or a regular address must be provided. Aborting.");
                return;
            }
            
            var blockchainClient = new BlockfrostClient(
                ConfigManager.GetConfigurationvalue("MainnetBaseUrl"),
                ConfigManager.GetConfigurationvalue("AuthenticationHeaderKey"),
                ConfigManager.GetConfigurationvalue("ApiKey")
            );

            if (stakeAddress == null && regularAddress != null)
            {
                var address = await blockchainClient.QueryAsync<CardanoAddress>($"addresses/{regularAddress}");
                stakeAddress = address.StakeAddress;
            }

            var rewardsReporter = new RewardsReporter(blockchainClient, new ReportWriter());
            await rewardsReporter.RunAsync(stakeAddress, orderBy, exportToCsv);
        }
    }
}
