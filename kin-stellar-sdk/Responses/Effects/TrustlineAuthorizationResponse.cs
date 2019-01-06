using Newtonsoft.Json;

namespace Kin.Stellar.Sdk.responses.effects
{
    /// <inheritdoc />
    public class TrustlineAuthorizationResponse : EffectResponse
    {
        [JsonProperty(PropertyName = "trustor")]
        public KeyPair Trustor { get; }

        [JsonProperty(PropertyName = "asset_type")]
        public string AssetType { get; }

        [JsonProperty(PropertyName = "asset_code")]
        public string AssetCode { get; }

        /// <inheritdoc />
        public TrustlineAuthorizationResponse(KeyPair trustor, string assetType, string assetCode)
        {
            Trustor = trustor;
            AssetType = assetType;
            AssetCode = assetCode;
        }
    }
}