using Newtonsoft.Json;

namespace Kin.Stellar.Sdk.responses.effects
{
    /// <inheritdoc />
    public class SignerEffectResponse : EffectResponse
    {
        [JsonProperty(PropertyName = "weight")]
        public int Weight { get; }

        [JsonProperty(PropertyName = "public_key")]
        public string PublicKey { get; }

        public SignerEffectResponse(int weight, string publicKey)
        {
            Weight = weight;
            PublicKey = publicKey;
        }
    }
}