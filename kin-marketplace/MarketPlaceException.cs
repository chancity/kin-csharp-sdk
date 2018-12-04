using System;
using System.Net;
using System.Net.Http;
using Kin.Marketplace.Models;

namespace Kin.Marketplace
{
    public class MarketPlaceException : Exception
    {
        public MarketPlaceError MarketPlaceError { get; }
        public HttpResponseMessage Response { get; }

        public MarketPlaceException(MarketPlaceError marketPlaceError, HttpResponseMessage response = null)
        {
            MarketPlaceError = marketPlaceError;
            Response = response;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(MarketPlaceError)}: {MarketPlaceError}";
        }
    }
}