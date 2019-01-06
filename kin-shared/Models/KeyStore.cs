using Newtonsoft.Json;

namespace Kin.Shared.Models
{
    public class KeyStore
    {
        [JsonProperty("pkey")]
        public string AccountId { get; private set; }
        [JsonProperty("seed")]
        public string Seed { get; private set; }


        public KeyStore(string accountId, string seed)
        {
            AccountId = accountId;
            Seed = seed;
        }
    }
}
