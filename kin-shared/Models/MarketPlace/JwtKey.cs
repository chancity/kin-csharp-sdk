using Newtonsoft.Json;

namespace Kin.Shared.Models.MarketPlace
{
    public class JwtKey
    {
        [JsonProperty("algorithm")]
        public string Algorithm { get; private set; }

        [JsonProperty("key")]
        public string Key { get; private set; }

        [JsonConstructor]
        private JwtKey() { }

        public JwtKey(string algorithm, string key)
        {
            Algorithm = algorithm;
            Key = key;
        }
    }
}