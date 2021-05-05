namespace ADARewardsReporter
{
    public class RewardsPerEpochSummary
    {
        public int Epoch { get; set; }
        public string RewardsReceivedOn { get; set; }
        public decimal Amount { get; set; }
    }
}