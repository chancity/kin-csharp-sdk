using Newtonsoft.Json;
using Kin.Stellar.Sdk.responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kin.Stellar.Sdk.responses
{
    public class FriendBotResponse 
    {
        [JsonProperty(PropertyName = "_links")]
        public FriendBotResponseLinks Links { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "extras")]
        public SubmitTransactionResponse.Extras Extras { get; set; }
        
        [JsonProperty(PropertyName = "detail")]
        public string Detail { get; set; }

        [JsonProperty(PropertyName = "hash")]
        public string Hash { get; set; }

        [JsonProperty(PropertyName = "ledger")]
        public string Ledger { get; set; }

        [JsonProperty(PropertyName = "envelope_xdr")]
        public string EnvelopeXdr { get; private set; }

        [JsonProperty(PropertyName = "result_xdr")]
        public string ResultXdr { get; private set; }

        [JsonProperty(PropertyName = "result_meta_xdr")]
        public string ResultMetaXdr { get; private set; }
    }

    public class FriendBotResponseLinks
    {
        [JsonProperty(PropertyName = "transaction")]
        public Link Transaction { get; private set; }
    }
}
