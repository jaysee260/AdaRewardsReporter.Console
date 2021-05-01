using System;
using System.Threading.Tasks;
using RestSharp;

namespace ADARewardsReporter
{
    public class BlockfrostClient : ICardanoBlockchainClient
    {
        public readonly string _networkBaseUrl;
        private readonly string _authenticationHeaderKey;
        private readonly string _apiKey;
        private readonly RestClient _client;

        public BlockfrostClient(string networkBaseUrl, string authenticationHeaderKey, string apiKey)
        {
            _networkBaseUrl = networkBaseUrl ?? throw new ArgumentNullException();
            _authenticationHeaderKey = authenticationHeaderKey ?? throw new ArgumentNullException();
            _apiKey = apiKey ?? throw new ArgumentNullException();
            _client = BuildClient();
        }

        public async Task<T> QueryAsync<T>(string resourceUri)
        {
            var request = new RestRequest(resourceUri);
            T response;
            try
            {
                response = await _client.GetAsync<T>(request);
            }
            catch (System.Exception exception)
            {
                System.Console.WriteLine($"{exception}");
                throw exception;
            }
            return response;
        }

        private RestClient BuildClient()
        {
            var client = new RestClient(_networkBaseUrl);
            client.AddDefaultHeader(_authenticationHeaderKey, _apiKey);
            return client;
        }
    }
}