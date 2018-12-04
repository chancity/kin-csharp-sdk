using Newtonsoft.Json;

namespace Kin.Stellar.Sdk.responses
{
    public class FriendBotResponseLinks
    {
        [JsonProperty(PropertyName = "transaction")]
        public Link Transaction { get; private set; }
    }
}