﻿using Newtonsoft.Json;

namespace Kin.Stellar.Sdk.responses
{
    /// <summary>
    ///     Represents account flags.
    /// </summary>
    public class Flags
    {
        public Flags(bool authRequired, bool authRevocable)
        {
            AuthRequired = authRequired;
            AuthRevocable = authRevocable;
        }

        [JsonProperty(PropertyName = "auth_required")]
        public bool AuthRequired { get; private set; }

        [JsonProperty(PropertyName = "auth_revocable")]
        public bool AuthRevocable { get; private set; }
    }
}