using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Kin.Marketplace.Models
{
    public class BlockChainData
    {
        [JsonProperty("transaction_id")]
        public string TransactionId { get; private set; }

        [JsonProperty("sender_address")]
        public string SenderAddress { get; private set; }

        [JsonProperty("recipient_address")]
        public string RecipientAddress { get; private set; }
    }
}
