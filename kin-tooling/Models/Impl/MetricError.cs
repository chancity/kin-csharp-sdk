using System;
using System.Collections.Generic;
using System.Text;

namespace Kin.Tooling.Models.Impl
{
    public class MetricError : IMetricError
    {
        public string Message { get; private set; }
        public long StatusCode { get; private set; }

        public MetricError(string message, long statusCode)
        {
            Message = message;
            StatusCode = statusCode;
        }
    }
}
