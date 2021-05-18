using Newtonsoft.Json;

namespace ADARewardsReporter.Models
{
    public class CardanoAddress
    {
        [JsonProperty("stake_address")]
        public string StakeAddress { get; set; }
        public string Type { get; set; }
    }
}