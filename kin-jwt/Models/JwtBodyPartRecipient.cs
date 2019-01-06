using Newtonsoft.Json;

namespace Kin.Jwt.Models
{
    internal class JwtBodyPartRecipient : JwtBodyPart
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        public JwtBodyPartRecipient(string userId, string title = "", string description = "")
        {
            UserId = userId;
            Title = title;
            Description = description;
        }
    }
}