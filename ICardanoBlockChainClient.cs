using System.Threading.Tasks;

namespace ADARewardsReporter
{
    public interface ICardanoBlockchainClient
    {
        Task<T> QueryAsync<T>(string resourceUri);
    }
}