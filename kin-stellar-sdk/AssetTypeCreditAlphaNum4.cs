﻿using Kin.Stellar.Sdk.xdr;

namespace Kin.Stellar.Sdk
{
    public class AssetTypeCreditAlphaNum4 : AssetTypeCreditAlphaNum
    {
        public AssetTypeCreditAlphaNum4(string code, KeyPair issuer) : base(code, issuer)
        {
            if (code.Length < 1 || code.Length > 4)
            {
                throw new AssetCodeLengthInvalidException();
            }
        }

        public override string GetType()
        {
            return "credit_alphanum4";
        }

        public override xdr.Asset ToXdr()
        {
            xdr.Asset thisXdr = new xdr.Asset();
            thisXdr.Discriminant = AssetType.Create(AssetType.AssetTypeEnum.ASSET_TYPE_CREDIT_ALPHANUM4);
            xdr.Asset.AssetAlphaNum4 credit = new xdr.Asset.AssetAlphaNum4();
            credit.AssetCode = Util.PaddedByteArray(Code, 4);
            AccountID accountID = new AccountID();
            accountID.InnerValue = Issuer.XdrPublicKey;
            credit.Issuer = accountID;
            thisXdr.AlphaNum4 = credit;
            return thisXdr;
        }
    }
}