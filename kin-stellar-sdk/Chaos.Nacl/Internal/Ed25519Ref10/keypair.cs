﻿using System;

namespace Kin.Stellar.Sdk.chaos.nacl.Internal.Ed25519Ref10
{
    internal static partial class Ed25519Operations
    {
        public static void CryptoSignKeypair(byte[] pk, int pkoffset, byte[] sk, int skoffset, byte[] seed,
            int seedoffset)
        {
            int i;

            Array.Copy(seed, seedoffset, sk, skoffset, 32);
            byte[] h = Sha512.Hash(sk, skoffset, 32); //ToDo: Remove alloc
            ScalarOperations.ScClamp(h, 0);

            GroupOperations.GeScalarmultBase(out GroupElementP3 a, h, 0);
            GroupOperations.ge_p3_tobytes(pk, pkoffset, ref a);

            for (i = 0; i < 32; ++i)
            {
                sk[skoffset + 32 + i] = pk[pkoffset + i];
            }

            CryptoBytes.Wipe(h);
        }
    }
}