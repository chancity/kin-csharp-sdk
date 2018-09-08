using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Kin.Marketplace.Models
{
    public class Cursors
    {
        [JsonProperty("after")]
        public string After { get; private set; }

        [JsonProperty("before")]
        public string Before { get; private set; }
    }
}