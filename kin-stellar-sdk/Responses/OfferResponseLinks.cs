using Newtonsoft.Json;

namespace Kin.Stellar.Sdk.responses
{
    public class OfferResponseLinks
    {
        [JsonProperty(PropertyName = "self")]
        public Link Self { get; private set; }

        [JsonProperty(PropertyName = "offer_maker")]
        public Link OfferMaker { get; private set; }

        public OfferResponseLinks(Link self, Link offerMaker)
        {
            Self = self;
            OfferMaker = offerMaker;
        }
    }
}