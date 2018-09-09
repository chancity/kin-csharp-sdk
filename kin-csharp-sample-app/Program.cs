using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kin.BlockChain;
using Kin.Jwt;
using Kin.Jwt.Models;
using Kin.Marketplace;
using Kin.Marketplace.Models;
using Kin.Shared.Models.Device;
using Kin.Shared.Models.MarketPlace;
using Newtonsoft.Json;
using stellar_dotnet_sdk;

namespace kin_csharp_sample_app
{
    internal class Program
    {


        private static void Main(string[] args)
        {
            try
            {

                Test().Wait();

            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;

                if (e is MarketPlaceException mex)
                {
                    Console.WriteLine(mex.MarketPlaceError);
                }
                else
                {
                    throw;
                }
            }

            Console.ReadLine();
        }

        public static async Task Test()
        {
            var firstKinClient = new SimpleKinClient();
            var secondKinClient = new SimpleKinClient();

            await Task.WhenAll(firstKinClient.FirstTest(), secondKinClient.FirstTest()).ConfigureAwait(false);

            await firstKinClient.SecondFailingTest(secondKinClient.UserId).ConfigureAwait(false);
            await secondKinClient.SecondFailingTest(firstKinClient.UserId).ConfigureAwait(false);
        }
    }
}