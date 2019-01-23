using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Kin.Backup.Extensions;
using Kin.Jwt;
using Kin.Marketplace;
using Kin.Shared.Models.Device;
using Kin.Shared.Models.MarketPlace;
using Kin.Stellar.Sdk;

namespace kin_csharp_sample_app
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Teser();
           // var _deviceInfo = new Information("KinCsharpClient", "BlazorWebApp", "Chrome", "Windows", "zomg");
           // JwtProviderBuilder jwtProviderBuilder = new JwtProviderBuilder();
           //
           // string payload = jwtProviderBuilder.Earn
           //     .AddOffer(Guid.NewGuid().ToString("N"), 20)
           //     .AddRecipient(Guid.NewGuid().ToString("N"), "title", "description")
           //     .Payload;
           //
           //
           // Bitmap bitmap = GetBitmapFromFile("./unnamed_chanceynick.png");
           // KeyPair keyPair = bitmap.ToKeyPair("chanceynick");
           //
           // Bitmap qrCode = keyPair.ToQrCode("chanceynick");
           // qrCode.Save("./test_chanceynick.png", ImageFormat.Png);

            Console.ReadLine();
        }

        private static void Teser()
        {
            Test().Wait();
        }

        public static async Task Test()
        {
            for (;;)
            {
                try
                {
                    SimpleKinClient firstKinClient = new SimpleKinClient();
                    //SimpleKinClient secondKinClient = new SimpleKinClient();

                    await firstKinClient.FirstTest();
                }
                catch
                {
                    //ignore
                }

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