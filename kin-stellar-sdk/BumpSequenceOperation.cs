using System;
using Kin.Stellar.Sdk.xdr;
using static Kin.Stellar.Sdk.xdr.Operation;
using Int64 = Kin.Stellar.Sdk.xdr.Int64;

namespace Kin.Stellar.Sdk
{
    public class BumpSequenceOperation : Operation
    {
        public long BumpTo { get; }

        public override OperationThreshold Threshold => OperationThreshold.Low;

        public BumpSequenceOperation(long bumpTo)
        {
            BumpTo = bumpTo;
        }

        public override OperationBody ToOperationBody()
        {
            BumpSequenceOp op = new BumpSequenceOp();
            Int64 bumpTo = new Int64 {InnerValue = BumpTo};
            SequenceNumber sequenceNumber = new SequenceNumber {InnerValue = bumpTo};

            op.BumpTo = sequenceNumber;

            OperationBody body = new OperationBody
            {
                Discriminant = OperationType.Create(OperationType.OperationTypeEnum.BUMP_SEQUENCE),
                BumpSequenceOp = op
            };

            return body;
        }

        public class Builder
        {
            private KeyPair _sourceAccount;
            public long BumpTo { get; }

            public Builder(BumpSequenceOp op)
            {
                BumpTo = op.BumpTo.InnerValue.InnerValue;
            }

            public Builder(long bumpTo)
            {
                BumpTo = bumpTo;
            }

            public Builder SetSourceAccount(KeyPair sourceAccount)
            {
                _sourceAccount = sourceAccount ??
                                 throw new ArgumentNullException(nameof(sourceAccount), "sourceAccount cannot be null");
                return this;
            }

            public BumpSequenceOperation Build()
            {
                BumpSequenceOperation operation = new BumpSequenceOperation(BumpTo);

                if (_sourceAccount != null)
                {
                    operation.SourceAccount = _sourceAccount;
                }

                return operation;
            }
        }
    }
}