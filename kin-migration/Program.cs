using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Kin.Stellar.Sdk;

namespace kin_migration
{
    class Program
    {
        private static HttpClient _httpClient;
        static void Main(string[] args)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.Proxy = new WebProxy("http://127.0.0.1:9000");
            _httpClient = new HttpClient(httpHandler);
        }
        public async Task<string> SendMigration(KeyPair keyPair)
        {
            string endpoint =
                $"https://migration-devplatform-playground.developers.kinecosystem.com/migrate?address={keyPair.AccountId}";
            string ret = null;

            try
            {
                using (HttpResponseMessage response = await _httpClient
                    .PostAsync(endpoint, new StringContent("", Encoding.UTF8, "application/json"))
                    .ConfigureAwait(false))

                {
                    ret = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
            catch (WebException wex)
            {
                if (wex?.Response == null)
                {
                    wex?.Response?.Dispose();
                    throw new ArgumentException($"{wex.Message} and no data returned...");
                }


                using (Stream stream = wex?.Response?.GetResponseStream())
                {
                    if (stream == null)
                    {
                        wex?.Response?.Dispose();
                        throw new ArgumentException($"{wex.Message} and no data returned...");
                    }

                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    ret = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            if (string.IsNullOrEmpty(ret))
            {
                throw new ArgumentException("No data returned...");
            }

            Console.WriteLine(ret);
            return ret;
        }


    }
}
