using Newtonsoft.Json;

namespace ADARewardsReporter.Models
{
    public class RewardHistoryEntry
    {
        public int Epoch { get; set; }
        public string Amount { get; set; }
        [JsonProperty("pool_id")]
        public string PoolId { get; set; }
    }
}