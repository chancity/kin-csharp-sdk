using System.Collections.Generic;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Kin.Marketplace.Models
{
    public class OfferList
    {
        [JsonProperty("offers")]
        public List<Offer> Offers { get; private set; }

        [JsonProperty("paging")]
        public Paging Paging { get; private set; }
    }
}