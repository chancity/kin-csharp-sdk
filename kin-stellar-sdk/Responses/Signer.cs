using System;
using Newtonsoft.Json;

namespace Kin.Stellar.Sdk.responses
{
    /// <summary>
    ///     Represents account signers.
    /// </summary>
    public class Signer
    {
        [Obsolete("Use Key instead.", false)]
        [JsonProperty(PropertyName = "public_key")]
        public string AccountId { get; private set; }

        [JsonProperty(PropertyName = "key")]
        public string Key { get; private set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; private set; }

        [JsonProperty(PropertyName = "weight")]
        public int Weight { get; private set; }

        public Signer(string key, string type, int? weight)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key), "key cannot be null");
            Type = type ?? throw new ArgumentNullException(nameof(type), "type cannot be null");
            Weight = weight ?? throw new ArgumentNullException(nameof(weight), "weight cannot be null");
        }
    }
}