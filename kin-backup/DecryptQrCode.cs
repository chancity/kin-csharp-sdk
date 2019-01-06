using System;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using Kin.Backup.LibSodium;
using Kin.Backup.Models;
using Kin.Shared.Models;
using Kin.Stellar.Sdk;
using Newtonsoft.Json;
using ZXing;
using ZXing.Common;
using ZXing.Windows.Compatibility;

namespace Kin.Backup
{
    public class DecryptQrCode
    {
        public static KeyPair ToKeyPair(Bitmap qrCodeImage, string passPhrase, bool disposeImage = true)
        {
            var encryptedKeyStore = BitmapToKeyStore(qrCodeImage);
            if (disposeImage) qrCodeImage.Dispose();
            return DecryptKeyStoreSeed(encryptedKeyStore, passPhrase);
        }

        private static EncryptedKeyStore BitmapToKeyStore(Bitmap image)
        {
            LuminanceSource source;
            source = new BitmapLuminanceSource(image);
            BinaryBitmap bitmap = new BinaryBitmap(new HybridBinarizer(source));
            Result result = new MultiFormatReader().decode(bitmap);
            return result != null ? JsonConvert.DeserializeObject<EncryptedKeyStore>(result.Text) : null;
        }

        private static KeyPair DecryptKeyStoreSeed(EncryptedKeyStore keyStore, string passPhrase)
        {
            SodiumCore.Init();

            byte[] passPhraseBytes = Encoding.UTF8.GetBytes(passPhrase);
            byte[] saltBytes = keyStore.EncryptionSalt.HexToBinary();
            byte[] keyHash = Shared.KeyHash(passPhraseBytes, saltBytes);
            byte[] seedBytes = keyStore.EncryptedSeed.HexToBinary();
            byte[] decryptedBytes = DecryptSecretSeed(seedBytes, keyHash);
            return KeyPair.FromSecretSeed(decryptedBytes);
        }

        private static byte[] DecryptSecretSeed(byte[] seedBytes, byte[] keyHash)
        {
            byte[] nonceBytes = new byte[24];
            byte[] cipherBytes = new byte[seedBytes.Length - nonceBytes.Length];
            Array.Copy(seedBytes, 0, nonceBytes, 0, nonceBytes.Length);
            Array.Copy(seedBytes, nonceBytes.Length, cipherBytes, 0, cipherBytes.Length);
            return SecretBox.Open(cipherBytes, nonceBytes, keyHash);
        }
    }
}
