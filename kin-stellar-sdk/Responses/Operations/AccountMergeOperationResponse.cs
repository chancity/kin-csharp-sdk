﻿using Newtonsoft.Json;

namespace Kin.Stellar.Sdk.responses.operations
{
    /// <summary>
    /// Represents AccountMerge operation response.
    /// See: https://www.stellar.org/developers/horizon/reference/resources/operation.html
    /// <seealso cref="requests.OperationsRequestBuilder"/>
    /// <seealso cref="Server"/>
    /// </summary>
    public class AccountMergeOperationResponse : OperationResponse
    {
        [JsonProperty(PropertyName = "account")]
        public KeyPair Account { get; }

        [JsonProperty(PropertyName = "into")] public KeyPair Into { get; }

        public AccountMergeOperationResponse(KeyPair account, KeyPair into)
        {
            Account = account;
            Into = into;
        }
    }
}