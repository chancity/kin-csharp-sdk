using System.Drawing;
using System.Threading.Tasks;
using Kin.Stellar.Sdk;

namespace Kin.Backup.Extensions
{
    public static class BitmapExtension
    {
        public static KeyPair ToKeyPair(this Bitmap qrCodeImage, string passPhrase, bool disposeImage = true)
        {
            return QrCode.ToKeyPair(qrCodeImage, passPhrase, disposeImage);
        }

        public static Task<KeyPair> ToKeyPairAsync(this Bitmap qrCodeImage, string passPhrase, bool disposeImage = true)
        {
            return Task.FromResult(QrCode.ToKeyPair(qrCodeImage, passPhrase, disposeImage));
        }
    }
}