using Newtonsoft.Json;

namespace Kin.Stellar.Sdk.responses
{
    /// <summary>
    /// 
    /// </summary>
    public class AssetResponseFlags
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "auth_required")]
        public bool AuthRequired { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "auth_revocable")]
        public bool AuthRevocable { get; set; }

        public AssetResponseFlags(bool authRequired, bool authRevocable)
        {
            AuthRequired = authRequired;
            AuthRevocable = authRevocable;
        }
    }
}