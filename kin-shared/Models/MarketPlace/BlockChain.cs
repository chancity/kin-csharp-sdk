using Newtonsoft.Json;

namespace Kin.Shared.Models.MarketPlace
{
    public class BlockChain
    {
        [JsonProperty("asset_code")]
        public string AssetCode { get; private set; }

        [JsonProperty("asset_issuer")]
        public string AssetIssuer { get; private set; }

        [JsonProperty("horizon_url")]
        public string HorizonUrl { get; private set; }

        [JsonProperty("network_passphrase")]
        public string NetworkPassphrase { get; private set; }
    }
}