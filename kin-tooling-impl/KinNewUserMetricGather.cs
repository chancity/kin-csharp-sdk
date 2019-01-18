using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kin.Marketplace.Models;
using Kin.Tooling.Core.Impl;

namespace kin_tooling_impl
{
    class KinNewUserMetricGather : AbstractMetricGather<AuthToken>
    {
        private readonly KinMarketPlaceUser _KinMarketPlaceUser;
        public KinNewUserMetricGather(KinMarketPlaceUser kinMarketPlaceUser)
        {
            _KinMarketPlaceUser = kinMarketPlaceUser;
        }
 
        protected override Task<AuthToken> InternalProcess(CancellationToken ctx)
        {
            return _KinMarketPlaceUser.SignInAndActivate();
        }
    }
}
