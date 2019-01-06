using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Kin.Marketplace;
using Kin.Backup.Extensions;
using Kin.Jwt;

namespace kin_csharp_sample_app
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var jwtProviderBuilder = new JwtProviderBuilder();

            var payload = jwtProviderBuilder.Earn
                .AddOffer(Guid.NewGuid().ToString("N"), 20)
                .AddRecipient(Guid.NewGuid().ToString("N"),"title","description")
                .Payload;


            Bitmap bitmap = GetBitmapFromFile("./unnamed_chanceynick.png");
            var keyPair = bitmap.ToKeyPair("chanceynick");

            Bitmap qrCode = keyPair.ToQrCode("chanceynick");
            qrCode.Save("./test_chanceynick.png", ImageFormat.Png);

            Console.ReadLine();
        }

        private static void Teser()
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

        public static Bitmap GetBitmapFromFile(string filePath)
        {
            try
            {
                return new Bitmap(Image.FromFile(filePath));
            }
            catch (Exception)
            {
                throw new FileNotFoundException($"Resource not found: {filePath}");
            }
        }
    }
}