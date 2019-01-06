namespace Kin.Stellar.Sdk.chaos.nacl.Internal.Ed25519Ref10
{
    internal static partial class GroupOperations
    {
        public static void GeToBytes(byte[] s, int offset, ref GroupElementP2 h)
        {
            FieldOperations.fe_invert(out FieldElement recip, ref h.Z);
            FieldOperations.fe_mul(out FieldElement x, ref h.X, ref recip);
            FieldOperations.fe_mul(out FieldElement y, ref h.Y, ref recip);
            FieldOperations.fe_tobytes(s, offset, ref y);
            s[offset + 31] ^= (byte) (FieldOperations.fe_isnegative(ref x) << 7);
        }
    }
}