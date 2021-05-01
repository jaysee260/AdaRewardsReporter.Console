using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADARewardsReporter.Models;

namespace ADARewardsReporter
{
    public class RewardsReporter
    {
        private readonly ICardanoBlockchainClient _blockchainClient;

        public RewardsReporter(ICardanoBlockchainClient blockchainClient)
        {
            _blockchainClient = blockchainClient ?? throw new ArgumentNullException();    
        }

        public async Task RunAsync(string stakeAddress)
        {
            var stakeRewardsUriTemplate = ConfigManager.GetConfigurationvalue("StakeRewardsUriTemplate");
            var stakeRewardsUri = string.Format(stakeRewardsUriTemplate, stakeAddress);
            var response = await _blockchainClient.QueryAsync<List<RewardHistoryEntry>>(stakeRewardsUri);
            
            var adaRewardsToDate = response.Select(x => x.Amount).Select(ConvertLovelaceToAda).Sum();
            System.Console.WriteLine($"Total rewards to date: {adaRewardsToDate} ADA\n");
            foreach (var reward in response)
            {
                System.Console.WriteLine($"Epoch {reward.Epoch}     {ConvertLovelaceToAda(reward.Amount)} ADA");
            }
        }

        private decimal ConvertLovelaceToAda(string amount)
        {
            var lovelaceToAdaRatio = 0.000001M;
            var amountInLovelace = Int32.Parse(amount);
            var amountInAda = (decimal)(amountInLovelace * lovelaceToAdaRatio);
            return amountInAda;
        }
    }
}