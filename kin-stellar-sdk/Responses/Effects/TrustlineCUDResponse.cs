using Newtonsoft.Json;

namespace Kin.Stellar.Sdk.responses.effects
{
    public abstract class TrustlineCUDResponse : EffectResponse
    {
        [JsonProperty(PropertyName = "limit")]
        public string Limit { get; }

        [JsonProperty(PropertyName = "asset_type")]
        public string AssetType { get; }

        [JsonProperty(PropertyName = "asset_code")]
        public string AssetCode { get; }

        [JsonProperty(PropertyName = "asset_issuer")]
        public string AssetIssuer { get; }

        public Asset Asset => Asset.CreateNonNativeAsset(AssetType, AssetIssuer, AssetCode);

        protected TrustlineCUDResponse(string limit, string assetType, string assetCode, string assetIssuer)
        {
            Limit = limit;
            AssetType = assetType;
            AssetCode = assetCode;
            AssetIssuer = assetIssuer;
        }
    }
}