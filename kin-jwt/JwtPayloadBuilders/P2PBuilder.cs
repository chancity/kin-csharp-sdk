using Kin.Jwt.Models;

namespace Kin.Jwt.JwtPayloadBuilders
{
    public class P2PBuilder : BaseBuilder
    {
        internal P2PBuilder(JwtProvider provider) : base(provider, SubjectNames.PayToUser, 3) { }

        public P2PBuilder AddOffer(string offerId, int amount)
        {
            JwtBodyPartOffer offer = new JwtBodyPartOffer(offerId, amount);

            KinJwtPayload<JwtBodyPartOffer> offerPayload =
                new KinJwtPayload<JwtBodyPartOffer>(ClaimNames.Offer, offer);
            AddPayload(offerPayload);
            return this;
        }

        public P2PBuilder AddSender(string userId, string title, string description)
        {
            JwtBodyPartSender sender = new JwtBodyPartSender(userId, title, description);

            KinJwtPayload<JwtBodyPartSender> senderPayload =
                new KinJwtPayload<JwtBodyPartSender>(ClaimNames.Sender, sender);
            AddPayload(senderPayload);
            return this;
        }

        public P2PBuilder AddRecipient(string userId, string title, string description)
        {
            JwtBodyPartRecipient recipient = new JwtBodyPartRecipient(userId, title, description);

            KinJwtPayload<JwtBodyPartRecipient> payload =
                new KinJwtPayload<JwtBodyPartRecipient>(ClaimNames.Recipient, recipient);
            AddPayload(payload);
            return this;
        }
    }
}