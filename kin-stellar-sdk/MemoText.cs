using System;
using System.Text;
using Kin.Stellar.Sdk.xdr;

namespace Kin.Stellar.Sdk
{
    public class MemoText : Memo
    {
        public string MemoTextValue { get; }

        public MemoText(string text)
        {
            MemoTextValue = text ?? throw new ArgumentNullException(nameof(text), "text cannot be null");

            int length = Encoding.UTF8.GetBytes(text).Length;

            if (length > 28)
            {
                throw new MemoTooLongException("text must be <= 28 bytes. length=" + length);
            }
        }

        public override xdr.Memo ToXdr()
        {
            xdr.Memo memo = new xdr.Memo();
            memo.Discriminant = MemoType.Create(MemoType.MemoTypeEnum.MEMO_TEXT);
            memo.Text = MemoTextValue ?? "none";
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

            MemoText memoText = (MemoText) o;
            return Equals(MemoTextValue, memoText.MemoTextValue);
        }

        public override int GetHashCode()
        {
            return HashCode.Start
                .Hash(MemoTextValue);
        }
    }
}