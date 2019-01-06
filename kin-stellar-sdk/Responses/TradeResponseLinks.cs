using Newtonsoft.Json;

namespace Kin.Stellar.Sdk.responses
{
    public class TradeResponseLinks
    {
        [JsonProperty(PropertyName = "base")] public Link Base;

        [JsonProperty(PropertyName = "counter")]
        public Link Counter;

        [JsonProperty(PropertyName = "operation")]
        public Link Operation;

        public TradeResponseLinks(Link baseLink, Link counterLink, Link operationLink)
        {
            Base = baseLink;
            Counter = counterLink;
            Operation = operationLink;
        }
    }
}