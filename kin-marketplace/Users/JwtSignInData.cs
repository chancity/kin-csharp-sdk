using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Kin.Marketplace.Models
{
    public class JwtSignInData : CommonSignInData
    {
        [JsonProperty("jwt")]
        public string Jwt { get; }

        public JwtSignInData(string deviceId, string walletAddress, string jwt) : base(deviceId, walletAddress)
        {
            Jwt = jwt;
            SignInType = "jwt";
        }
    }
}