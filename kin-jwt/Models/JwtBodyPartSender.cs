namespace Kin.Jwt.Models
{
    internal class JwtBodyPartSender : JwtBodyPartRecipient
    {
        public JwtBodyPartSender(string userId, string title = "", string description = "") : base(userId, title,
            description) { }
    }
}