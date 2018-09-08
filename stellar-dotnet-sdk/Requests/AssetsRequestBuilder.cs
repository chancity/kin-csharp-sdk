﻿using stellar_dotnet_sdk.responses;
using stellar_dotnet_sdk.responses.page;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace stellar_dotnet_sdk.requests
{
    /// <summary>
    /// 
    /// </summary>
    public class AssetsRequestBuilder : RequestBuilderExecutePageable<AssetsRequestBuilder, AssetResponse>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverUri"></param>
        public AssetsRequestBuilder(Uri serverUri, HttpClient httpClient)
            : base(serverUri, "assets", httpClient)
        {
        }

        /// <summary>
        /// Code of the Asset to filter by
        /// </summary>
        /// <param name="assetCode"></param>
        /// <returns></returns>
        public AssetsRequestBuilder AssetCode(string assetCode)
        {
            _uriBuilder.SetQueryParam("asset_code", assetCode);
            return this;
        }

        /// <summary>
        /// Issuer of the Asset to filter by
        /// </summary>
        /// <param name="assetIssuer"></param>
        /// <returns></returns>
        public AssetsRequestBuilder AssetIssuer(string assetIssuer)
        {
            _uriBuilder.SetQueryParam("asset_issuer", assetIssuer);
            return this;
        }

    }
}
