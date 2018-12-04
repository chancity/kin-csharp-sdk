using Newtonsoft.Json;

namespace Kin.Jwt.Models
{
    internal class JwtBodyPartOffer : JwtBodyPart
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }


        public JwtBodyPartOffer(string id, int amount)
        {
            Id = id;
            Amount = amount;
        }
    }
}