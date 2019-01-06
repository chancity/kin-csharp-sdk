using System;
using System.Text;
using Kin.Backup.LibSodium;
using Kin.Backup.LibSodium.Exceptions;

namespace Sodium
{
    /// <summary>Hashes passwords using the argon2i and scrypt algorithm</summary>
    internal class PasswordHash
    {
        /// <summary>Represents predefined and useful limits for ArgonHashBinary() and ArgonHashString().</summary>
        public enum StrengthArgon
        {
            /// <summary>For interactive sessions (fast: uses 32MB of RAM).</summary>
            INTERACTIVE,

            /// <summary>For normal use (moderate: uses 128MB of RAM).</summary>
            MODERATE,

            /// <summary>For highly sensitive data (slow: uses 512MB of RAM).</summary>
            SENSITIVE
        }

        /// <summary>
        ///     Is an identifier for the algorithm to use and should
        ///     be currently set to crypto_pwhash_ALG_DEFAULT.
        /// </summary>
        private const int ARGON_ALGORITHM_DEFAULT = 2;

        private const uint ARGON_STRBYTES = 128U;
        private const uint ARGON_SALTBYTES = 16U;

        private const long ARGON_OPSLIMIT_INTERACTIVE = 4;
        private const long ARGON_OPSLIMIT_MODERATE = 6;
        private const long ARGON_OPSLIMIT_SENSITIVE = 8;

        private const int ARGON_MEMLIMIT_INTERACTIVE = 33554432;
        private const int ARGON_MEMLIMIT_MODERATE = 134217728;
        private const int ARGON_MEMLIMIT_SENSITIVE = 536870912;

        /// <summary>Generates a random 16 byte salt for the Argon2i algorithm.</summary>
        /// <returns>Returns a byte array with 16 random bytes</returns>
        public static byte[] ArgonGenerateSalt()
        {
            return SodiumCore.GetRandomBytes((int) ARGON_SALTBYTES);
        }

        /// <summary>
        ///     Derives a secret key of any size from a password and a salt.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="opsLimit">Represents a maximum amount of computations to perform.</param>
        /// <param name="memLimit">Is the maximum amount of RAM that the function will use, in bytes.</param>
        /// <param name="outputLength">The length of the computed output array.</param>
        /// <returns>Returns a byte array of the given size.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="SaltOutOfRangeException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        public static byte[] ArgonHashBinary(byte[] password, byte[] salt, long opsLimit, int memLimit,
            long outputLength = ARGON_SALTBYTES)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password", "Password cannot be null");
            }

            if (salt == null)
            {
                throw new ArgumentNullException("salt", "Salt cannot be null");
            }

            if (salt.Length != ARGON_SALTBYTES)
            {
                throw new SaltOutOfRangeException(string.Format("Salt must be {0} bytes in length.", ARGON_SALTBYTES));
            }

            if (opsLimit < 2)
            {
                throw new ArgumentOutOfRangeException("opsLimit",
                    "opsLimit the number of passes, has to be at least 3");
            }

            if (memLimit <= 0)
            {
                throw new ArgumentOutOfRangeException("memLimit", "memLimit cannot be zero or negative");
            }

            if (outputLength <= 0)
            {
                throw new ArgumentOutOfRangeException("outputLength", "OutputLength cannot be zero or negative");
            }

            byte[] buffer = new byte[outputLength];

            SodiumCore.Init();

            int ret = SodiumLibrary.crypto_pwhash(buffer, buffer.Length, password, password.GetLongLength(0), salt,
                opsLimit, memLimit, ARGON_ALGORITHM_DEFAULT);

            if (ret != 0)
            {
                throw new OutOfMemoryException(
                    "Internal error, hash failed (usually because the operating system refused to allocate the amount of requested memory).");
            }

            return buffer;
        }

        /// <summary>Derives a secret key of any size from a password and a salt.</summary>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="limit">The limit for computation.</param>
        /// <param name="outputLength">The length of the computed output array.</param>
        /// <returns>Returns a byte array of the given size.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="SaltOutOfRangeException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        public static byte[] ArgonHashBinary(string password, string salt,
            StrengthArgon limit = StrengthArgon.INTERACTIVE, long outputLength = ARGON_SALTBYTES)
        {
            return ArgonHashBinary(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(salt), limit, outputLength);
        }

        /// <summary>Derives a secret key of any size from a password and a salt.</summary>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="limit">The limit for computation.</param>
        /// <param name="outputLength">The length of the computed output array.</param>
        /// <returns>Returns a byte array of the given size.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="SaltOutOfRangeException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        public static byte[] ArgonHashBinary(byte[] password, byte[] salt,
            StrengthArgon limit = StrengthArgon.INTERACTIVE, long outputLength = ARGON_SALTBYTES)
        {
            int memLimit;
            long opsLimit;

            switch (limit)
            {
                case StrengthArgon.INTERACTIVE:
                    opsLimit = ARGON_OPSLIMIT_INTERACTIVE;
                    memLimit = ARGON_MEMLIMIT_INTERACTIVE;
                    break;
                case StrengthArgon.MODERATE:
                    opsLimit = ARGON_OPSLIMIT_MODERATE;
                    memLimit = ARGON_MEMLIMIT_MODERATE;
                    break;
                case StrengthArgon.SENSITIVE:
                    opsLimit = ARGON_OPSLIMIT_SENSITIVE;
                    memLimit = ARGON_MEMLIMIT_SENSITIVE;
                    break;
                default:
                    opsLimit = ARGON_OPSLIMIT_INTERACTIVE;
                    memLimit = ARGON_MEMLIMIT_INTERACTIVE;
                    break;
            }

            return ArgonHashBinary(password, salt, opsLimit, memLimit, outputLength);
        }

        /// <summary>
        ///     Derives a secret key of any size from a password and a salt.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="opsLimit">Represents a maximum amount of computations to perform.</param>
        /// <param name="memLimit">Is the maximum amount of RAM that the function will use, in bytes.</param>
        /// <param name="outputLength">The length of the computed output array.</param>
        /// <returns>Returns a byte array of the given size.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="SaltOutOfRangeException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        public static byte[] ArgonHashBinary(string password, string salt, long opsLimit, int memLimit,
            long outputLength = ARGON_SALTBYTES)
        {
            byte[] pass = Encoding.UTF8.GetBytes(password);
            byte[] saltAsBytes = Encoding.UTF8.GetBytes(salt);

            return ArgonHashBinary(pass, saltAsBytes, opsLimit, memLimit, outputLength);
        }

        /// <summary>Returns the hash in a string format, which includes the generated salt.</summary>
        /// <param name="password">The password.</param>
        /// <param name="limit">The limit for computation.</param>
        /// <returns>Returns an zero-terminated ASCII encoded string of the computed password and hash.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        public static string ArgonHashString(string password, StrengthArgon limit = StrengthArgon.INTERACTIVE)
        {
            int memLimit;
            long opsLimit;

            switch (limit)
            {
                case StrengthArgon.INTERACTIVE:
                    opsLimit = ARGON_OPSLIMIT_INTERACTIVE;
                    memLimit = ARGON_MEMLIMIT_INTERACTIVE;
                    break;
                case StrengthArgon.MODERATE:
                    opsLimit = ARGON_OPSLIMIT_MODERATE;
                    memLimit = ARGON_MEMLIMIT_MODERATE;
                    break;
                case StrengthArgon.SENSITIVE:
                    opsLimit = ARGON_OPSLIMIT_SENSITIVE;
                    memLimit = ARGON_MEMLIMIT_SENSITIVE;
                    break;
                default:
                    opsLimit = ARGON_OPSLIMIT_INTERACTIVE;
                    memLimit = ARGON_MEMLIMIT_INTERACTIVE;
                    break;
            }

            return ArgonHashString(password, opsLimit, memLimit);
        }

        /// <summary>Returns the hash in a string format, which includes the generated salt.</summary>
        /// <param name="password">The password.</param>
        /// <param name="opsLimit">Represents a maximum amount of computations to perform.</param>
        /// <param name="memLimit">Is the maximum amount of RAM that the function will use, in bytes.</param>
        /// <returns>Returns an zero-terminated ASCII encoded string of the computed password and hash.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        public static string ArgonHashString(string password, long opsLimit, int memLimit)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password", "Password cannot be null");
            }

            if (opsLimit < 3)
            {
                throw new ArgumentOutOfRangeException("opsLimit",
                    "opsLimit the number of passes, has to be at least 3");
            }

            if (memLimit <= 0)
            {
                throw new ArgumentOutOfRangeException("memLimit", "memLimit cannot be zero or negative");
            }

            byte[] buffer = new byte[ARGON_STRBYTES];
            byte[] pass = Encoding.UTF8.GetBytes(password);

            SodiumCore.Init();
            int ret = SodiumLibrary.crypto_pwhash_str(buffer, pass, pass.GetLongLength(0), opsLimit, memLimit);

            if (ret != 0)
            {
                throw new OutOfMemoryException(
                    "Internal error, hash failed (usually because the operating system refused to allocate the amount of requested memory).");
            }

            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>Verifies that a hash generated with ArgonHashString matches the supplied password.</summary>
        /// <param name="hash">The hash.</param>
        /// <param name="password">The password.</param>
        /// <returns><c>true</c> on success; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool ArgonHashStringVerify(string hash, string password)
        {
            return ArgonHashStringVerify(Encoding.UTF8.GetBytes(hash), Encoding.UTF8.GetBytes(password));
        }

        /// <summary>Verifies that a hash generated with ArgonHashString matches the supplied password.</summary>
        /// <param name="hash">The hash.</param>
        /// <param name="password">The password.</param>
        /// <returns><c>true</c> on success; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool ArgonHashStringVerify(byte[] hash, byte[] password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password", "Password cannot be null");
            }

            if (hash == null)
            {
                throw new ArgumentNullException("hash", "Hash cannot be null");
            }

            int ret = SodiumLibrary.crypto_pwhash_str_verify(hash, password, password.GetLongLength(0));

            return ret == 0;
        }
    }
}