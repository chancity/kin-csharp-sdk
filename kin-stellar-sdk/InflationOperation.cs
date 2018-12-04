using System;
using Kin.Stellar.Sdk.xdr;

namespace Kin.Stellar.Sdk
{
    public class InflationOperation : Operation
    {
        public override xdr.Operation.OperationBody ToOperationBody()
        {
            var body = new xdr.Operation.OperationBody
            {
                Discriminant = OperationType.Create(OperationType.OperationTypeEnum.INFLATION)
            };

            return body;
        }

        public class Builder
        {
            private KeyPair mSourceAccount;

            /// <summary>
            ///     Sets the source account for this operation.
            /// </summary>
            /// <param name="sourceAccount">The operation's source account.</param>
            /// <returns>Builder object so you can chain methods.</returns>
            public Builder SetSourceAccount(KeyPair sourceAccount)
            {
                mSourceAccount = sourceAccount ?? throw new ArgumentNullException(nameof(sourceAccount), "sourceAccount cannot be null");
                return this;
            }

            /// <summary>
            ///     Builds an operation
            /// </summary>
            public InflationOperation Build()
            {
                var operation = new InflationOperation();
                if (mSourceAccount != null)
                    operation.SourceAccount = mSourceAccount;
                return operation;
            }
        }
    }
}