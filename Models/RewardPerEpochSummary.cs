using System.ComponentModel;

namespace ADARewardsReporter
{
    public class RewardsPerEpochSummary
    {
        public int Epoch { get; set; }
        [DisplayName("Rewards Received On")]
        public string RewardsReceivedOn { get; set; }
        [DisplayName("Stake Pool")]
        public string StakePool { get; set; }
        [DisplayName("Amount (ADA)")]
        public decimal Amount { get; set; }
    }
}