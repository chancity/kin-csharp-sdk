using System;
using System.Net.Http;
using Kin.Stellar.Sdk.responses;

namespace Kin.Stellar.Sdk.requests
{
    /// <inheritdoc />
    public class OffersRequestBuilder : RequestBuilderExecutePageable<OffersRequestBuilder, OfferResponse>
    {
        public OffersRequestBuilder(Uri serverURI, HttpClient httpClient) :
            base(serverURI, "offers", httpClient) { }

        /// <summary>
        ///     Builds request to GET /accounts/{account}/offers
        ///     See: https://www.stellar.org/developers/horizon/reference/offers-for-account.html
        /// </summary>
        /// <param name="account">Account for which to get offers</param>
        /// <returns></returns>
        public OffersRequestBuilder ForAccount(KeyPair account)
        {
            account = account ?? throw new ArgumentNullException(nameof(account), "account cannot be null");
            SetSegments("accounts", account.AccountId, "offers");
            return this;
        }
    }
}