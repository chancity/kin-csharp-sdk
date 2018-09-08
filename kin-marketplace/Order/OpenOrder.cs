using System;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Kin.Marketplace.Models
{
    public class OpenOrder : BaseOrder
    {
        [JsonProperty("expiration_date")]
        public DateTime ExpirationDate { get; private set; }
    }
}