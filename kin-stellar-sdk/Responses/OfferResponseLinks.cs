using Newtonsoft.Json;

namespace Kin.Stellar.Sdk.responses
{
    public class OfferResponseLinks
    {
        public OfferResponseLinks(Link self, Link offerMaker)
        {
            Self = self;
            OfferMaker = offerMaker;
        }

        [JsonProperty(PropertyName = "self")] public Link Self { get; private set; }

        [JsonProperty(PropertyName = "offer_maker")]
        public Link OfferMaker { get; private set; }
    }
}