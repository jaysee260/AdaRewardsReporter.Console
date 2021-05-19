using System.ComponentModel;
using CsvHelper.Configuration.Attributes;

namespace ADARewardsReporter
{
    public class RewardsPerEpochSummary
    {
        public int Epoch { get; set; }
        [DisplayName("Rewards Received On")]
        [Name("Rewards Received On")]
        public string RewardsReceivedOn { get; set; }
        [DisplayName("Stake Pool")]
        [Name("Stake Pool")]
        public string StakePool { get; set; }
        [DisplayName("Amount (ADA)")]
        [Name("Amount (ADA)")]
        public decimal Amount { get; set; }
    }
}