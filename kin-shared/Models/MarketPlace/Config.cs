using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kin.Shared.Models.MarketPlace
{
    public class Config
    {
        [JsonProperty("jwt_keys")]
        public Dictionary<string, JwtKey> JwtKeys { get; private set; }

        [JsonProperty("blockchain")]
        public BlockChain BlockChain { get; private set; }

        [JsonProperty("bi_service")]
        public string BiService { get; private set; }

        [JsonProperty("webview")]
        public string WebView { get; private set; }

        [JsonProperty("environment_name")]
        public string EnvironmentName { get; private set; }

        [JsonProperty("ecosystem_service")]
        public string EcosystemService { get; private set; }
    }
}