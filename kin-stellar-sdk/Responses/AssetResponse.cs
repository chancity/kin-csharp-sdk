using Newtonsoft.Json;

namespace Kin.Stellar.Sdk.responses
{
    /// <summary>
    /// </summary>
    public class AssetResponse : Response, IPagingToken
    {
        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "_links")]
        public AssetResponseLinks Links { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "asset_type")]
        public string AssetType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "asset_code")]
        public string AssetCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "asset_issuer")]
        public string AssetIssuer { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ammount")]
        public string Amount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "num_accounts")]
        public long NumAccounts { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "flags")]
        public AssetResponseFlags Flags { get; set; }

        /// <summary>
        /// </summary>
        public Asset Asset => Asset.Create(AssetType, AssetCode, AssetIssuer);

        public AssetResponse(string assetType, string assetCode, string assetIssuer, string pagingToken, string amount,
            int numAccounts, AssetResponseFlags flags, AssetResponseLinks links)
        {
            AssetType = assetType;
            AssetCode = assetCode;
            AssetIssuer = assetIssuer;
            PagingToken = pagingToken;
            Amount = amount;
            NumAccounts = numAccounts;
            Flags = flags;
            Links = links;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "paging_token")]
        public string PagingToken { get; set; }
    }
}