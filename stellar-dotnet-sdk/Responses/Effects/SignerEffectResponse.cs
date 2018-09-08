﻿using Newtonsoft.Json;

namespace stellar_dotnet_sdk.responses.effects
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