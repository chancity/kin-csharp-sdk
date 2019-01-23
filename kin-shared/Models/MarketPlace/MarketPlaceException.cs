using System;
using System.Net.Http;

namespace Kin.Shared.Models.MarketPlace
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