namespace Kin.Stellar.Sdk.chaos.nacl.Internal.Ed25519Ref10
{
    internal static partial class GroupOperations
    {
        /*
        r = p
        */
        public static void ge_p3_to_cached(out GroupElementCached r, ref GroupElementP3 p)
        {
            Kin.Stellar.Sdk.chaos.nacl.Internal.Ed25519Ref10.FieldOperations.fe_add(out r.YplusX, ref p.Y, ref p.X);
            Kin.Stellar.Sdk.chaos.nacl.Internal.Ed25519Ref10.FieldOperations.fe_sub(out r.YminusX, ref p.Y, ref p.X);
            r.Z = p.Z;
            Kin.Stellar.Sdk.chaos.nacl.Internal.Ed25519Ref10.FieldOperations.fe_mul(out r.T2d, ref p.T, ref Kin.Stellar.Sdk.chaos.nacl.Internal.Ed25519Ref10.LookupTables.d2);
        }
    }
}