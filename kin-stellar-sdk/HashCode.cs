using System.Collections.Generic;

namespace Kin.Stellar.Sdk
{
    public static class HashCode
    {
        public const int Start = 17;

        public static int Hash<T>(this int hash, T obj)
        {
            int h = EqualityComparer<T>.Default.GetHashCode(obj);
            return unchecked(hash * 31 + h);
        }
    }
}