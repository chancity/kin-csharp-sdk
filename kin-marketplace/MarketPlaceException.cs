using System;
using Kin.Marketplace.Models;

namespace Kin.Marketplace
{
    public class MarketPlaceException : Exception
    {
        public MarketPlaceError MarketPlaceError { get; }

        public MarketPlaceException(MarketPlaceError marketPlaceError)
        {
            MarketPlaceError = marketPlaceError;
        }
    }
}