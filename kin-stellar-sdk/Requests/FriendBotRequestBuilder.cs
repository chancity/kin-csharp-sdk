using System;
using System.Net.Http;
using System.Threading.Tasks;
using Kin.Stellar.Sdk.responses;

namespace Kin.Stellar.Sdk.requests
{
    public class FriendBotRequestBuilder : RequestBuilder<FriendBotRequestBuilder>
    {
        /// <summary>
        /// </summary>
        /// <param name="serverUri"></param>
        public FriendBotRequestBuilder(Uri serverUri, HttpClient httpClient)
            : base(serverUri, "friendbot", httpClient)
        {
            if (Network.Current == null)
            {
                throw new NotSupportedException("FriendBot requires the TESTNET Network to be set explicitly.");
            }

            if (Network.IsPublicNetwork(Network.Current))
            {
                throw new NotSupportedException("FriendBot is only supported on the TESTNET Network.");
            }
        }

        public FriendBotRequestBuilder FundAccount(KeyPair account)
        {
            UriBuilder.SetQueryParam("addr", account.AccountId);
            return this;
        }

        /// <Summary>
        ///     Build and execute request.
        /// </Summary>
        public async Task<FriendBotResponse> Execute()
        {
            return await Execute<FriendBotResponse>(BuildUri());
        }
    }
}