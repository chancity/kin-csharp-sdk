using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Kin.Marketplace.Models
{
    public abstract class CommonSignInData
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; }

        [JsonProperty("wallet_address")]
        public string WalletAddress { get; }

        [JsonProperty("sign_in_type")]
        public string SignInType { get; protected set; }

        protected CommonSignInData(string deviceId, string walletAddress)
        {
            DeviceId = deviceId;
            WalletAddress = walletAddress;
        }
    }
}