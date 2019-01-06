using System;
using Kin.Stellar.Sdk.xdr;

namespace Kin.Stellar.Sdk
{
    public class MemoId : Memo
    {
        public long IdValue { get; }

        public MemoId(long id)
        {
            if (id < 0)
            {
                throw new ArgumentException("id must be a positive number");
            }

            IdValue = id;
        }

        public override xdr.Memo ToXdr()
        {
            xdr.Memo memo = new xdr.Memo();
            memo.Discriminant = MemoType.Create(MemoType.MemoTypeEnum.MEMO_ID);
            Uint64 idXdr = new Uint64();
            idXdr.InnerValue = IdValue;
            memo.Id = idXdr;
            return memo;
        }

        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }

            if (o == null || GetType() != o.GetType())
            {
                return false;
            }

            MemoId memoId = (MemoId) o;
            return IdValue == memoId.IdValue;
        }

        public override int GetHashCode()
        {
            return HashCode.Start
                .Hash(IdValue);
        }
    }
}