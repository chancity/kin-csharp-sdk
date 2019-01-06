using Kin.Stellar.Sdk.xdr;

namespace Kin.Stellar.Sdk
{
    public class MemoHash : MemoHashAbstract
    {
        public MemoHash(byte[] bytes) : base(bytes) { }

        public MemoHash(string hexString) : base(hexString) { }

        public override xdr.Memo ToXdr()
        {
            xdr.Memo memo = new xdr.Memo();
            memo.Discriminant = MemoType.Create(MemoType.MemoTypeEnum.MEMO_HASH);

            Hash hash = new Hash();
            hash.InnerValue = MemoBytes;

            memo.Hash = hash;

            return memo;
        }
    }
}