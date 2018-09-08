using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Kin.Marketplace.Models
{
    public class MarketPlaceError
    {
        [JsonProperty("error")]
        public string Error { get; private set; }

        [JsonProperty("message")]
        public string Message { get; private set; }

        [JsonProperty("code")]
        public int Code { get; private set; }
    }
}