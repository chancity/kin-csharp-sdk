﻿namespace Kin.Stellar.Sdk.chaos.nacl.Internal.Ed25519Ref10
{
    internal static partial class GroupOperations
    {
        public static void ge_p3_0(out GroupElementP3 h)
        {
            Kin.Stellar.Sdk.chaos.nacl.Internal.Ed25519Ref10.FieldOperations.fe_0(out h.X);
            Kin.Stellar.Sdk.chaos.nacl.Internal.Ed25519Ref10.FieldOperations.fe_1(out h.Y);
            Kin.Stellar.Sdk.chaos.nacl.Internal.Ed25519Ref10.FieldOperations.fe_1(out h.Z);
            Kin.Stellar.Sdk.chaos.nacl.Internal.Ed25519Ref10.FieldOperations.fe_0(out h.T);
        }
    }
}