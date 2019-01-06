using System.Drawing;
using System.Threading.Tasks;
using Kin.Stellar.Sdk;

namespace Kin.Backup.Extensions
{
    public static class KeyPairExtension
    {
        public static Bitmap ToQrCode(this KeyPair keyPair, string passPhrase)
        {
            return QrCode.ToQrCode(keyPair, passPhrase);
        }

        public static Task<Bitmap> ToQrCodeAsync(this KeyPair keyPair, string passPhrase)
        {
            return Task.FromResult(QrCode.ToQrCode(keyPair, passPhrase));
        }
    }
}