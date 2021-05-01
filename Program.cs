using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADARewardsReporter.Models;
using Microsoft.Extensions.Configuration;
using NDesk.Options;
using RestSharp;

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

            var config = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
                                .Build();

            var authenticationHeaderKey = config["AuthenticationHeaderKey"];
            var apiKey = config["ApiKey"];
            var mainnetBaseUrl = config["MainnetBaseUrl"];
            var stakeRewardsUriTemplate = config["StakeRewardsUriTemplate"];

            var client = new RestClient(mainnetBaseUrl);
            client.AddDefaultHeader(authenticationHeaderKey, apiKey);
            var stakeRewardsUri = string.Format(stakeRewardsUriTemplate, stakeAddress);
            var request = new RestRequest(stakeRewardsUri);
            var response = await client.GetAsync<List<RewardHistoryEntry>>(request);
            
            var adaRewardsToDate = response.Select(x => x.Amount).Select(ConvertLovelaceToAda).Sum();
            System.Console.WriteLine($"Total rewards to date: {adaRewardsToDate} ADA\n");
            foreach (var reward in response)
            {
                System.Console.WriteLine($"Epoch {reward.Epoch}     {ConvertLovelaceToAda(reward.Amount)} ADA");
            }
        }

        public static decimal ConvertLovelaceToAda(string amount)
        {
            var lovelaceToAdaRatio = 0.000001M;
            var amountInLovelace = Int32.Parse(amount);
            var amountInAda = (decimal)(amountInLovelace * lovelaceToAdaRatio);
            return amountInAda;
        }
    }
}
