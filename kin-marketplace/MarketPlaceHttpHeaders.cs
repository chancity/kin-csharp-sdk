using System.Collections.Generic;
using System.Net.Http.Headers;
using Kin.Shared.Models.Device;

namespace Kin.Marketplace
{
    internal class MarketPlaceHttpHeaders : HttpHeaders
    {
        public const string XRequestId = "X-REQUEST-ID";
        public const string XDeviceId = "X-DEVICE-ID";
        public const string XSdkVersion = "X-SDK-VERSION";
        public const string XDeviceModel = "X-DEVICE-MODEL";
        public const string XDeviceManufacturer = "X-DEVICE-MANUFACTURER";
        public const string XOs = "X-OS";

        private readonly List<KeyValuePair<string, string>> _values;

        public IEnumerable<KeyValuePair<string, string>> Values => _values;

        public MarketPlaceHttpHeaders(Information info)
        {
            _values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(XDeviceId, info.XDeviceId),
                new KeyValuePair<string, string>(XSdkVersion, info.XSdkVersion),
                new KeyValuePair<string, string>(XDeviceModel, info.XDeviceModel),
                new KeyValuePair<string, string>(XDeviceManufacturer, info.XDeviceManufacturer),
                new KeyValuePair<string, string>(XOs, info.XOs)
            };
        }
    }
}