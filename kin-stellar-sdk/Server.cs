using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Kin.Stellar.Sdk.requests;
using Kin.Stellar.Sdk.responses;

namespace Kin.Stellar.Sdk
{
    public class Server : IDisposable
    {
        private readonly Uri _serverUri;

        public static HttpClient HttpClient { get; set; }

        public AccountsRequestBuilder Accounts => new AccountsRequestBuilder(_serverUri, HttpClient);

        public AssetsRequestBuilder Assets => new AssetsRequestBuilder(_serverUri, HttpClient);

        public EffectsRequestBuilder Effects => new EffectsRequestBuilder(_serverUri, HttpClient);

        public LedgersRequestBuilder Ledgers => new LedgersRequestBuilder(_serverUri, HttpClient);

        public OffersRequestBuilder Offers => new OffersRequestBuilder(_serverUri, HttpClient);

        public OperationsRequestBuilder Operations => new OperationsRequestBuilder(_serverUri, HttpClient);

        public OrderBookRequestBuilder OrderBook => new OrderBookRequestBuilder(_serverUri, HttpClient);

        public TradesRequestBuilder Trades => new TradesRequestBuilder(_serverUri, HttpClient);

        public PathsRequestBuilder Paths => new PathsRequestBuilder(_serverUri, HttpClient);

        public PaymentsRequestBuilder Payments => new PaymentsRequestBuilder(_serverUri, HttpClient);

        public TransactionsRequestBuilder Transactions => new TransactionsRequestBuilder(_serverUri, HttpClient);

        public FriendBotRequestBuilder TestNetFriendBot => new FriendBotRequestBuilder(_serverUri, HttpClient);

        public TradesAggregationRequestBuilder TradeAggregations =>
            new TradesAggregationRequestBuilder(_serverUri, HttpClient);

        public Server(string uri, HttpClient httpClient)
        {
            HttpClient = httpClient;
            _serverUri = new Uri(uri);
        }

        public Server(string uri)
            : this(uri, new HttpClient()) { }

        public void Dispose()
        {
            HttpClient?.Dispose();
        }

        public RootResponse Root()
        {
            ResponseHandler<RootResponse> responseHandler = new ResponseHandler<RootResponse>();

            HttpResponseMessage response = HttpClient.GetAsync(_serverUri).Result;

            return responseHandler.HandleResponse(response).Result;
        }

        public async Task<SubmitTransactionResponse> SubmitTransaction(Transaction transaction)
        {
            Uri transactionUri = new UriBuilder(_serverUri).SetPath("/transactions").Uri;

            List<KeyValuePair<string, string>> paramsPairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("tx", transaction.ToEnvelopeXdrBase64())
            };

            HttpResponseMessage response =
                await HttpClient.PostAsync(transactionUri, new FormUrlEncodedContent(paramsPairs.ToArray()));

            if (response.Content != null)
            {
                string responseString = await response.Content.ReadAsStringAsync();

                SubmitTransactionResponse submitTransactionResponse =
                    JsonSingleton.GetInstance<SubmitTransactionResponse>(responseString);
                return submitTransactionResponse;
            }

            return null;
        }

        public async Task<SubmitTransactionResponse> SubmitTransaction(string transactionEnvelopeBase64)
        {
            Uri transactionUri = new UriBuilder(_serverUri).SetPath("/transactions").Uri;

            List<KeyValuePair<string, string>> paramsPairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("tx", transactionEnvelopeBase64)
            };

            HttpResponseMessage response =
                await HttpClient.PostAsync(transactionUri, new FormUrlEncodedContent(paramsPairs.ToArray()));

            if (response.Content != null)
            {
                string responseString = await response.Content.ReadAsStringAsync();

                SubmitTransactionResponse submitTransactionResponse =
                    JsonSingleton.GetInstance<SubmitTransactionResponse>(responseString);
                return submitTransactionResponse;
            }

            return null;
        }
    }
}