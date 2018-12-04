using System;
using System.Threading.Tasks;
using Kin.Marketplace;
using Kin.Marketplace.Models;
using Newtonsoft.Json;

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
            for (;;)
            {
                SimpleKinClient firstKinClient = new SimpleKinClient();
                //SimpleKinClient secondKinClient = new SimpleKinClient();

                await firstKinClient.FirstTest();
            }
        }
    }
}