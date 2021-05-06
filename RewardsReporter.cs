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
            // Get rewards history
            var rewardsHistory = await GetRewardsHistoryAsync(stakeAddress);
            var rewardsHistoryOrderedByEpochInAsc = rewardsHistory.OrderBy(x => x.Epoch).ToList();

            // Get epochs details
            var epochNumbers = rewardsHistory.Select(x => x.Epoch);
            var epochs = await GetEpochsDetailsAsync(epochNumbers);
            var epochsOrderedInAsc = epochs.OrderBy(x => x.Epoch).ToList();

            // Produce Rewards Per Epoch Summary
            var rewardsSummary = ProduceRewardsPerEpochSummary(rewardsHistoryOrderedByEpochInAsc, epochsOrderedInAsc);

            // Print summary
            PrintSummaryToConsole(rewardsSummary);
        }

        private async Task<IEnumerable<RewardPerEpoch>> GetRewardsHistoryAsync(string stakeAddress)
        {
            var stakeRewardsUriTemplate = ConfigManager.GetConfigurationvalue("StakeRewardsUriTemplate");
            var stakeRewardsUri = string.Format(stakeRewardsUriTemplate, stakeAddress);
            var rewardsHistory = await _blockchainClient.QueryAsync<List<RewardPerEpoch>>(stakeRewardsUri);
            return rewardsHistory;
        }

        private async Task<IEnumerable<CardanoEpoch>> GetEpochsDetailsAsync(IEnumerable<int> epochNumbers)
        {
            var epochsTasks = epochNumbers.Select(e => _blockchainClient.QueryAsync<CardanoEpoch>($"epochs/{e}"));
            var epochsTasksResolved = await Task.WhenAll(epochsTasks);
            return epochsTasksResolved;
        }

        private IEnumerable<RewardsPerEpochSummary> ProduceRewardsPerEpochSummary(List<RewardPerEpoch> rewardsHistory, List<CardanoEpoch> epochs)
        {
            var length = rewardsHistory.Count() == epochs.Count()
                ? rewardsHistory.Count()
                : throw new Exception();

            List<RewardsPerEpochSummary> rewardsSummary = new List<RewardsPerEpochSummary>();
            for (int i = 0; i < length; i++)
            {
                var rewardSummary = new RewardsPerEpochSummary
                {
                    Epoch = epochs[i].Epoch,
                    Amount = ConvertLovelaceToAda(rewardsHistory[i].Amount),
                    RewardsReceivedOn = ConvertUnixTimestampToRewardsReceivedDate(epochs[i].EndTime)
                };
                rewardsSummary.Add(rewardSummary);
            }

            return rewardsSummary;
        }

        private decimal ConvertLovelaceToAda(string amount)
        {
            var lovelaceToAdaRate = 0.000001M;
            var amountInLovelace = Int32.Parse(amount);
            var amountInAda = (decimal)(amountInLovelace * lovelaceToAdaRate);
            return amountInAda;
        }

        private string ConvertUnixTimestampToRewardsReceivedDate(int epochEndTime)
        {
            // Rewards for a given epoch X are paid out at the end of the next epoch, X+1
            var daysInOneEpoch = 5;
            var epochEndDate = DateTimeOffset.FromUnixTimeSeconds(epochEndTime);
            var nextEpochEndDate = epochEndDate.AddDays(daysInOneEpoch);
            var shortDateString = nextEpochEndDate.DateTime.ToShortDateString();
            return shortDateString;
        }

        private void PrintSummaryToConsole(IEnumerable<RewardsPerEpochSummary> rewardsSummary)
        {
            var adaRewardsToDate = rewardsSummary.Select(x => x.Amount).Sum();
            System.Console.WriteLine($"Total rewards to date: {adaRewardsToDate} ADA\n");
            TableBuilder.BuildTableFrom(rewardsSummary).Write();
        }
    }
}