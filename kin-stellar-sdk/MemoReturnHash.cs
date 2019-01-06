using Kin.Stellar.Sdk.xdr;

namespace Kin.Stellar.Sdk
{
    public class MemoReturnHash : MemoHashAbstract
    {
        public MemoReturnHash(byte[] bytes) : base(bytes) { }

        public MemoReturnHash(string hexString) : base(hexString) { }

        public override xdr.Memo ToXdr()
        {
            xdr.Memo memo = new xdr.Memo();
            memo.Discriminant = MemoType.Create(MemoType.MemoTypeEnum.MEMO_RETURN);

            Hash hash = new Hash();
            hash.InnerValue = MemoBytes;

            memo.RetHash = hash;

            return memo;
        }
    }
}