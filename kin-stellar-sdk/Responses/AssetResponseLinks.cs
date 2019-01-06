using Newtonsoft.Json;

namespace Kin.Stellar.Sdk.responses
{
    /// <summary>
    /// </summary>
    public class AssetResponseLinks
    {
        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "toml")]
        public Link Toml { get; set; }

        public AssetResponseLinks(Link toml)
        {
            Toml = toml;
        }
    }
}