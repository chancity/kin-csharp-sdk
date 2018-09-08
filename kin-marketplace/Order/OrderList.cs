using System.Collections.Generic;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Kin.Marketplace.Models
{
    public class OrderList
    {
        [JsonProperty("Orders")]
        public List<Order> Orders { get; private set; }

        [JsonProperty("Paging")]
        public Paging Paging { get; private set; }
    }
}