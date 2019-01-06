using System;
using System.Security.Cryptography;
using System.Text;
using Kin.Backup.LibSodium.Exceptions;

namespace Kin.Backup.LibSodium
{
    /// <summary>Create and Open Secret Boxes.</summary>
    internal static class SecretBox
    {
        private const int KEY_BYTES = 32;
        private const int NONCE_BYTES = 24;
        private const int MAC_BYTES = 16;

        /// <summary>Generates a random 32 byte key.</summary>
        /// <returns>Returns a byte array with 32 random bytes</returns>
        public static byte[] GenerateKey()
        {
            return SodiumCore.GetRandomBytes(KEY_BYTES);
        }

        /// <summary>Generates a random 24 byte nonce.</summary>
        /// <returns>Returns a byte array with 24 random bytes</returns>
        public static byte[] GenerateNonce()
        {
            return SodiumCore.GetRandomBytes(NONCE_BYTES);
        }

        /// <summary>Creates a Secret Box</summary>
        /// <param name="message">Hex-encoded string to be encrypted.</param>
        /// <param name="nonce">The 24 byte nonce.</param>
        /// <param name="key">The 32 byte key.</param>
        /// <returns>The encrypted message.</returns>
        /// <exception cref="KeyOutOfRangeException"></exception>
        /// <exception cref="NonceOutOfRangeException"></exception>
        /// <exception cref="CryptographicException"></exception>
        public static byte[] Create(string message, byte[] nonce, byte[] key)
        {
            return Create(Encoding.UTF8.GetBytes(message), nonce, key);
        }

        /// <summary>Creates a Secret Box</summary>
        /// <param name="message">The message.</param>
        /// <param name="nonce">The 24 byte nonce.</param>
        /// <param name="key">The 32 byte key.</param>
        /// <returns>The encrypted message.</returns>
        /// <exception cref="KeyOutOfRangeException"></exception>
        /// <exception cref="NonceOutOfRangeException"></exception>
        /// <exception cref="CryptographicException"></exception>
        public static byte[] Create(byte[] message, byte[] nonce, byte[] key)
        {
            //validate the length of the key
            if (key == null || key.Length != KEY_BYTES)
            {
                throw new KeyOutOfRangeException("key", key == null ? 0 : key.Length,
                    string.Format("key must be {0} bytes in length.", KEY_BYTES));
            }

            //validate the length of the nonce
            if (nonce == null || nonce.Length != NONCE_BYTES)
            {
                throw new NonceOutOfRangeException("nonce", nonce == null ? 0 : nonce.Length,
                    string.Format("nonce must be {0} bytes in length.", NONCE_BYTES));
            }

            byte[] buffer = new byte[MAC_BYTES + message.Length];

            SodiumCore.Init();
            int ret = SodiumLibrary.crypto_secretbox_easy(buffer, message, message.Length, nonce, key);

            if (ret != 0)
            {
                throw new CryptographicException("Failed to create SecretBox");
            }

            return buffer;
        }

        /// <summary>Opens a Secret Box</summary>
        /// <param name="cipherText">Hex-encoded string to be opened.</param>
        /// <param name="nonce">The 24 byte nonce.</param>
        /// <param name="key">The 32 byte nonce.</param>
        /// <returns>The decrypted text.</returns>
        /// <exception cref="KeyOutOfRangeException"></exception>
        /// <exception cref="NonceOutOfRangeException"></exception>
        /// <exception cref="CryptographicException"></exception>
        public static byte[] Open(string cipherText, byte[] nonce, byte[] key)
        {
            return Open(cipherText.HexToBinary(), nonce, key);
        }

        /// <summary>Opens a Secret Box</summary>
        /// <param name="cipherText">The cipherText.</param>
        /// <param name="nonce">The 24 byte nonce.</param>
        /// <param name="key">The 32 byte nonce.</param>
        /// <returns>The decrypted text.</returns>
        /// <exception cref="KeyOutOfRangeException"></exception>
        /// <exception cref="NonceOutOfRangeException"></exception>
        /// <exception cref="CryptographicException"></exception>
        public static byte[] Open(byte[] cipherText, byte[] nonce, byte[] key)
        {
            //validate the length of the key
            if (key == null || key.Length != KEY_BYTES)
            {
                throw new KeyOutOfRangeException("key", key == null ? 0 : key.Length,
                    string.Format("key must be {0} bytes in length.", KEY_BYTES));
            }

            //validate the length of the nonce
            if (nonce == null || nonce.Length != NONCE_BYTES)
            {
                throw new NonceOutOfRangeException("nonce", nonce == null ? 0 : nonce.Length,
                    string.Format("nonce must be {0} bytes in length.", NONCE_BYTES));
            }

            //check to see if there are MAC_BYTES of leading nulls, if so, trim.
            //this is required due to an error in older versions.
            if (cipherText[0] == 0)
            {
                //check to see if trim is needed
                bool trim = true;

                for (int i = 0; i < MAC_BYTES - 1; i++)
                {
                    if (cipherText[i] != 0)
                    {
                        trim = false;
                        break;
                    }
                }

                //if the leading MAC_BYTES are null, trim it off before going on.
                if (trim)
                {
                    byte[] temp = new byte[cipherText.Length - MAC_BYTES];
                    Array.Copy(cipherText, MAC_BYTES, temp, 0, cipherText.Length - MAC_BYTES);

                    cipherText = temp;
                }
            }

            byte[] buffer = new byte[cipherText.Length - MAC_BYTES];
            SodiumCore.Init();
            int ret = SodiumLibrary.crypto_secretbox_open_easy(buffer, cipherText, cipherText.Length, nonce, key);

            if (ret != 0)
            {
                throw new CryptographicException("Failed to open SecretBox");
            }

            return buffer;
        }
    }
}