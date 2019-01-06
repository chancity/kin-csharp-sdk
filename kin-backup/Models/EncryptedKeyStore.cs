using Newtonsoft.Json;

namespace Kin.Backup.Models
{
    internal class EncryptedKeyStore
    {
        [JsonProperty("pkey")]
        public string AccountId { get; private set; }
        [JsonProperty("seed")]
        public string EncryptedSeed { get; private set; }
        [JsonProperty("salt")]
        public string EncryptionSalt { get; private set; }

        public EncryptedKeyStore(string accountId, string encryptedSeed, string encryptionSalt)
        {
            AccountId = accountId;
            EncryptedSeed = encryptedSeed;
            EncryptionSalt = encryptionSalt;
        }
    }
}
