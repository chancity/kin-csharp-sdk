using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Kin.Marketplace.Models
{
    public class WhiteListSignInData : CommonSignInData
    {
        [JsonProperty("user_id")]
        public string UserId { get; }

        [JsonProperty("api_key")]
        public string ApiKey { get; }

        public WhiteListSignInData(string deviceId, string walletAddress, string userId, string apiKey) : base(deviceId,
            walletAddress)
        {
            UserId = userId;
            ApiKey = apiKey;
            SignInType = "whitelist";
        }
    }
}