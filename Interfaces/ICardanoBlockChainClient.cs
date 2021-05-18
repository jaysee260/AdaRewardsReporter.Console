using System.Threading.Tasks;

namespace ADARewardsReporter.Interfaces
{
    public interface ICardanoBlockchainClient
    {
        Task<T> QueryAsync<T>(string resourceUri);
    }
}