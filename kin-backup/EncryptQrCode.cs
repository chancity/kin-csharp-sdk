using System;
using System.Drawing;
using System.Text;
using Kin.Backup.LibSodium;
using Kin.Backup.Models;
using Kin.Stellar.Sdk;
using Newtonsoft.Json;
using Sodium;
using ZXing;
using ZXing.Windows.Compatibility;

namespace Kin.Backup
{
    internal partial class QrCode
    {
        public static Bitmap ToQrCode(KeyPair keyPair, string passPhrase)
        {
            EncryptedKeyStore encryptedKeyStore = ToKeyStore(keyPair, passPhrase);
            return KeyStoreToQrCode(JsonConvert.SerializeObject(encryptedKeyStore));
        }

        private static Bitmap KeyStoreToQrCode(string content)
        {
            BarcodeWriter writer = new BarcodeWriter {Format = BarcodeFormat.QR_CODE};
            writer.Options.Height = 930;
            writer.Options.Width = 930;
            Bitmap result = writer.Write(content);
            return new Bitmap(result);
        }

        private static EncryptedKeyStore ToKeyStore(KeyPair keyPair, string passPhrase)
        {
            byte[] passPhraseBytes = Encoding.UTF8.GetBytes(passPhrase);
            byte[] encryptionSalt = PasswordHash.ArgonGenerateSalt();
            byte[] keyHash = Shared.KeyHash(passPhraseBytes, encryptionSalt);
            byte[] encryptedSeed = EncryptSecretSeed(keyPair.SeedBytes, keyHash);

            return new EncryptedKeyStore(keyPair.AccountId, encryptedSeed.BinaryToHex(),
                encryptionSalt.BinaryToHex());
        }

        private static byte[] EncryptSecretSeed(byte[] seedBytes, byte[] keyHash)
        {
            byte[] nonceBytes = SecretBox.GenerateNonce();
            byte[] cipherText = SecretBox.Create(seedBytes, nonceBytes, keyHash);
            byte[] encryptedSeed = new byte[cipherText.Length + nonceBytes.Length];
            Array.Copy(nonceBytes, 0, encryptedSeed, 0, nonceBytes.Length);
            Array.Copy(cipherText, 0, encryptedSeed, nonceBytes.Length, cipherText.Length);
            return encryptedSeed;
        }
    }
}