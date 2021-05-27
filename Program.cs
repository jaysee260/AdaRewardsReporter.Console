using System;
using System.Threading.Tasks;
using AdaRewardsReporter.Core.Utils;
using NDesk.Options;
using AdaRewardsReporter.Core;
using AdaRewardsReporter.Core.Models;
using AdaRewardsReporter.Console.Utils;

namespace AdaRewardsReporter.Console
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
                System.Console.WriteLine("A stake address or a regular address must be provided. Aborting.");
                return;
            }
            

            var blockchainClient = new BlockfrostClient(ConfigManager.GetConfiguration());
            
            if (stakeAddress == null && regularAddress != null)
            {
                var address = await blockchainClient.QueryAsync<CardanoAddress>($"addresses/{regularAddress}");
                stakeAddress = address.StakeAddress;
            }

            var rewardsReporter = new RewardsReporter(blockchainClient);
            var rewardsSummary = await rewardsReporter.GenerateReportAsync(stakeAddress, orderBy);

            TableHelper.PrintSummaryToConsole(rewardsSummary);

            if (exportToCsv)
            {
                new ReportWriter().WriteReport(rewardsSummary);
            }
        }
    }
}
