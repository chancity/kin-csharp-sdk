﻿using Newtonsoft.Json;

namespace Kin.Stellar.Sdk.federation
{
    /// <summary>
    ///     Object to hold a response from a federation server.
    ///     See https://www.stellar.org/developers/learn/concepts/federation.html
    /// </summary>
    public class FederationResponse
    {
        [JsonProperty(PropertyName = "stellar_address")]
        public string StellarAddress { get; private set; }

        [JsonProperty(PropertyName = "account_id")]
        public string AccountId { get; private set; }

        [JsonProperty(PropertyName = "memo_type")]
        public string MemoType { get; private set; }

        [JsonProperty(PropertyName = "memo")]
        public string Memo { get; private set; }

        public FederationResponse(string stellarAddress, string accountId, string memoType, string memo)
        {
            StellarAddress = stellarAddress;
            AccountId = accountId;
            MemoType = memoType;
            Memo = memo;
        }
    }
}