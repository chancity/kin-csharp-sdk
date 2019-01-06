using System;
using System.Text;
using Newtonsoft.Json;

namespace Kin.Stellar.Sdk.responses
{
    public class AccountDataResponse
    {
        [JsonProperty(PropertyName = "value")]
        public string Value { get; private set; }

        public string ValueDecoded
        {
            get
            {
                byte[] data = Convert.FromBase64String(Value);
                return Encoding.UTF8.GetString(data);
            }
        }

        public AccountDataResponse(string value)
        {
            Value = value;
        }
    }
}