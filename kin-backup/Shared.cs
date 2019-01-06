using Sodium;

namespace Kin.Backup
{
    internal class Shared
    {
        public static byte[] KeyHash(byte[] passPhraseBytes, byte[] saltBytes)
        {
            var keyHash = PasswordHash.ArgonHashBinary(passPhraseBytes, saltBytes, 2, 67108864, 32);
            return keyHash;
        }
    }
}
