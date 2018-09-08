using System;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Kin.Marketplace.Models
{
    public class AuthToken
    {
        [JsonProperty("token")]
        public string Token { get; private set; }

        [JsonProperty("activated")]
        public bool Activated { get; private set; }

        [JsonProperty("expiration_date")]
        public DateTime ExpirationDate { get; private set; }

        [JsonProperty("app_id")]
        public string AppId { get; private set; }

        [JsonProperty("user_id")]
        public string UserId { get; private set; }

        [JsonProperty("ecosystem_user_id")]
        public string EcosystemUserId { get; private set; }
    }
}