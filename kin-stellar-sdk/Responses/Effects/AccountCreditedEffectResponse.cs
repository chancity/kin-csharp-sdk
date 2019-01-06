﻿using Newtonsoft.Json;

namespace Kin.Stellar.Sdk.responses.effects
{
    /// <summary>
    ///     Represents account_credited effect response.
    ///     See: https://www.stellar.org/developers/horizon/reference/resources/effect.html
    ///     <seealso cref="requests.EffectsRequestBuilder" />
    ///     <seealso cref="Server" />
    /// </summary>
    public class AccountCreditedEffectResponse : EffectResponse
    {
        [JsonProperty(PropertyName = "amount")]
        public string Amount { get; }

        [JsonProperty(PropertyName = "asset_type")]
        public string AssetType { get; }

        [JsonProperty(PropertyName = "asset_code")]
        public string AssetCode { get; }

        [JsonProperty(PropertyName = "asset_issuer")]
        public string AssetIssuer { get; }

        public Asset Asset => Asset.CreateNonNativeAsset(AssetType, AssetIssuer, AssetCode);

        /// <inheritdoc />
        public AccountCreditedEffectResponse(string amount, string assetType, string assetCode, string assetIssuer)
        {
            Amount = amount;
            AssetType = assetType;
            AssetCode = assetCode;
            AssetIssuer = assetIssuer;
        }
    }
}