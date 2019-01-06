using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Kin.Marketplace.Models
{
    public class OrderResult
    {
        [JsonProperty("type")]
        public string Type { get; private set; }

        [JsonProperty("coupon_code")]
        public string CouponCode { get; private set; }

        [JsonProperty("jwt")]
        public string Jwt { get; private set; }
    }
}