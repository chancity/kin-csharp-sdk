using System;

namespace Kin.Stellar.Sdk.federation
{
    /// <summary>
    ///     Federation server was not found in stellar.toml file.
    /// </summary>
    internal class NoFederationServerException : Exception { }
}