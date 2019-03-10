using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Kin.Marketplace.Models
{
    public class WhiteList
    {
        [JsonProperty("network_id")]
        public string NetworkPassphrase { get; set; }

        [JsonProperty("tx_envelope")]
        public string TransactionPayload { get; set; }

        public WhiteList(string transactionPayload, string networkPassphrase)
        {
            TransactionPayload = transactionPayload;
            NetworkPassphrase = networkPassphrase;
        }
    }
}
