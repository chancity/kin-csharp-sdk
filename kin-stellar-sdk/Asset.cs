﻿using System;
using Kin.Stellar.Sdk.xdr;

namespace Kin.Stellar.Sdk
{
    /// <summary>
    ///     Asset class represents an asset, either the native asset (XLM) or a asset code / issuer account ID pair.
    ///     An asset code describes an asset code and issuer pair. In the case of the native asset XLM, the issuer will be
    ///     null.
    /// </summary>
    public abstract class Asset
    {
        /// <summary>
        ///     Create an asset base on the parameters.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <param name="issuer"></param>
        /// <returns>Asset</returns>
        public static Asset Create(string type, string code, string issuer)
        {
            if (type == "native")
            {
                return new AssetTypeNative();
            }

            return CreateNonNativeAsset(code, KeyPair.FromAccountId(issuer));
        }

        /// <summary>
        ///     Creates one of <seealso cref="AssetTypeCreditAlphaNum4" /> or <seealso cref="AssetTypeCreditAlphaNum12" /> object
        ///     based on a code length
        /// </summary>
        /// <param name="code">Asset code</param>
        /// <param name="issuer">Asset issuer</param>
        public static Asset CreateNonNativeAsset(string code, KeyPair issuer)
        {
            if (code.Length >= 1 && code.Length <= 4)
            {
                return new AssetTypeCreditAlphaNum4(code, issuer);
            }

            if (code.Length >= 5 && code.Length <= 12)
            {
                return new AssetTypeCreditAlphaNum12(code, issuer);
            }

            throw new AssetCodeLengthInvalidException();
        }

        /// <summary>
        ///     Generates Asset object from a given XDR object
        /// </summary>
        /// <param name="thisXdr"></param>
        public static Asset FromXdr(xdr.Asset thisXdr)
        {
            switch (thisXdr.Discriminant.InnerValue)
            {
                case AssetType.AssetTypeEnum.ASSET_TYPE_NATIVE:
                    return new AssetTypeNative();
                case AssetType.AssetTypeEnum.ASSET_TYPE_CREDIT_ALPHANUM4:
                    string assetCode4 = Util.PaddedByteArrayToString(thisXdr.AlphaNum4.AssetCode);
                    KeyPair issuer4 = KeyPair.FromXdrPublicKey(thisXdr.AlphaNum4.Issuer.InnerValue);
                    return new AssetTypeCreditAlphaNum4(assetCode4, issuer4);
                case AssetType.AssetTypeEnum.ASSET_TYPE_CREDIT_ALPHANUM12:
                    string assetCode12 = Util.PaddedByteArrayToString(thisXdr.AlphaNum12.AssetCode);
                    KeyPair issuer12 = KeyPair.FromXdrPublicKey(thisXdr.AlphaNum12.Issuer.InnerValue);
                    return new AssetTypeCreditAlphaNum12(assetCode12, issuer12);
                default:
                    throw new ArgumentException("Unknown asset type " + thisXdr.Discriminant.InnerValue);
            }
        }

        /// <summary>
        ///     Returns asset type. Possible types:
        ///     <ul>
        ///         <li>native See <see cref="AssetTypeNative" /> for more information.</li>
        ///         <li>credit_alphanum4 See <see cref="AssetTypeCreditAlphaNum4" /> for more information.</li>
        ///         <li>credit_alphanum12 See <see cref="AssetTypeCreditAlphaNum12" /> for more information.</li>
        ///     </ul>
        /// </summary>
        public new abstract string GetType();

        /// <summary>
        ///     Generates XDR object from a given Asset object
        /// </summary>
        public abstract xdr.Asset ToXdr();

        /// <summary>
        ///     Creates one of AssetTypeCreditAlphaNum4 or AssetTypeCreditAlphaNum12 object based on a code length
        /// </summary>
        public static Asset CreateNonNativeAsset(string assetType, string accountId, string code)
        {
            if (assetType == "native")
            {
                return new AssetTypeNative();
            }

            KeyPair issuer = KeyPair.FromAccountId(accountId);
            return CreateNonNativeAsset(code, issuer);
        }
    }
}