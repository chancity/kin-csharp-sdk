using Newtonsoft.Json;

namespace Kin.Jwt.Models
{
    internal class JwtBodyPartOffer : JwtBodyPart
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }


        public JwtBodyPartOffer(string id, string amount)
        {
            Id = id;
            Amount = amount;
        }
    }
}