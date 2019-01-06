using System.Drawing;
using Kin.Stellar.Sdk;
using System.Threading.Tasks;

namespace Kin.Backup.Extensions
{
    public static class BitmapExtension
    {
        public static KeyPair ToKeyPair(this Bitmap qrCodeImage, string passPhrase, bool disposeImage = true)
        {
            return DecryptQrCode.ToKeyPair(qrCodeImage, passPhrase, disposeImage);
        }

        public static Task<KeyPair> ToKeyPairAsync(this Bitmap qrCodeImage, string passPhrase, bool disposeImage = true)
        {
            return Task.FromResult(DecryptQrCode.ToKeyPair(qrCodeImage, passPhrase, disposeImage));
        }
    }
}
