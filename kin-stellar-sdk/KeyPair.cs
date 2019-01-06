﻿using System;
using System.Linq;
using System.Security.Cryptography;
using Kin.Stellar.Sdk.chaos.nacl;
using Kin.Stellar.Sdk.xdr;

namespace Kin.Stellar.Sdk
{
    /// <summary>
    ///     <see cref="KeyPair" /> represents public (and secret) keys of the account.
    ///     Currently <see cref="KeyPair" /> only supports ed25519 but in a future this class can be abstraction layer for
    ///     other public-key signature systems.
    /// </summary>
    public class KeyPair
    {
        /// <summary>
        ///     The public key.
        /// </summary>
        public byte[] PublicKey { get; }

        /// <summary>
        ///     The private key.
        /// </summary>
        public byte[] PrivateKey { get; }

        /// <summary>
        ///     The bytes of the Secret Seed
        /// </summary>
        public byte[] SeedBytes { get; }

        /// <summary>
        ///     AccountId
        /// </summary>
        public string AccountId => StrKey.EncodeStellarAccountId(PublicKey);

        /// <summary>
        ///     Address
        /// </summary>
        public string Address => StrKey.EncodeCheck(StrKey.VersionByte.ACCOUNT_ID, PublicKey);

        /// <summary>
        ///     SecretSeed
        /// </summary>
        public string SecretSeed => StrKey.EncodeStellarSecretSeed(SeedBytes);

        /// <summary>
        ///     XDR Signature Hint
        /// </summary>
        public SignatureHint SignatureHint
        {
            get
            {
                XdrDataOutputStream stream = new XdrDataOutputStream();
                AccountID accountId = new AccountID(XdrPublicKey);
                AccountID.Encode(stream, accountId);
                byte[] bytes = stream.ToArray();
                int length = bytes.Length;
                byte[] signatureHintBytes = bytes.Skip(length - 4).Take(4).ToArray();

                SignatureHint signatureHint = new SignatureHint(signatureHintBytes);
                return signatureHint;
            }
        }

        /// <summary>
        ///     XDR Public Key
        /// </summary>
        public PublicKey XdrPublicKey
        {
            get
            {
                PublicKey publicKey = new PublicKey
                {
                    Discriminant = new PublicKeyType
                        {InnerValue = PublicKeyType.PublicKeyTypeEnum.PUBLIC_KEY_TYPE_ED25519}
                };

                Uint256 uint256 = new Uint256(PublicKey);
                publicKey.Ed25519 = uint256;

                return publicKey;
            }
        }

        /// <summary>
        ///     XDR Signer Key
        /// </summary>
        public SignerKey XdrSignerKey
        {
            get
            {
                SignerKey signerKey = new SignerKey
                {
                    Discriminant = new SignerKeyType
                        {InnerValue = SignerKeyType.SignerKeyTypeEnum.SIGNER_KEY_TYPE_ED25519}
                };

                Uint256 uint256 = new Uint256(PublicKey);
                signerKey.Ed25519 = uint256;

                return signerKey;
            }
        }

        /// <summary>
        ///     Creates a new Keypair object from public key.
        /// </summary>
        /// <param name="publicKey"></param>
        public KeyPair(byte[] publicKey)
            : this(publicKey, null, null) { }

        /// <summary>
        ///     Creates a new Keypair instance from secret. This can either be secret key or secret seed depending on underlying
        ///     public-key signature system. Currently Keypair only supports ed25519.
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="privateKey"></param>
        /// <param name="seed"></param>
        public KeyPair(byte[] publicKey, byte[] privateKey, byte[] seed)
        {
            PublicKey = publicKey ?? throw new ArgumentNullException(nameof(publicKey), "publicKey cannot be null");
            PrivateKey = privateKey;
            SeedBytes = seed;
        }

        /// <summary>
        ///     Returns a KeyPair from a Public Key
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns>
        ///     <see cref="KeyPair" />
        /// </returns>
        public static KeyPair FromXdrPublicKey(PublicKey publicKey)
        {
            return FromPublicKey(publicKey.Ed25519.InnerValue);
        }

        /// <summary>
        ///     Returns a KeyPair from an XDR SignerKey
        /// </summary>
        /// <param name="signerKey"></param>
        /// <returns>
        ///     <see cref="KeyPair" />
        /// </returns>
        public static KeyPair FromXdrSignerKey(SignerKey signerKey)
        {
            return FromPublicKey(signerKey.Ed25519.InnerValue);
        }

        /// <summary>
        ///     Returns true if this Keypair is capable of signing
        /// </summary>
        /// <returns></returns>
        public bool CanSign()
        {
            return PrivateKey != null;
        }

        /// <summary>
        ///     Creates a new Stellar KeyPair from a strkey encoded Stellar secret seed.
        /// </summary>
        /// <param name="seed">eed Char array containing strkey encoded Stellar secret seed.</param>
        /// <returns>
        ///     <see cref="KeyPair" />
        /// </returns>
        public static KeyPair FromSecretSeed(string seed)
        {
            byte[] bytes = StrKey.DecodeStellarSecretSeed(seed);
            return FromSecretSeed(bytes);
        }

        /// <summary>
        ///     Creates a new Stellar keypair from a raw 32 byte secret seed.
        /// </summary>
        /// <param name="seed">seed The 32 byte secret seed.</param>
        /// <returns>
        ///     <see cref="KeyPair" />
        /// </returns>
        public static KeyPair FromSecretSeed(byte[] seed)
        {
            Ed25519.KeyPairFromSeed(out byte[] publicKey, out byte[] privateKey, seed);

            return new KeyPair(publicKey, privateKey, seed);
        }

        /// <summary>
        ///     Creates a new Stellar KeyPair from a strkey encoded Stellar account ID.
        /// </summary>
        /// <param name="accountId">accountId The strkey encoded Stellar account ID.</param>
        /// <returns>
        ///     <see cref="KeyPair" />
        /// </returns>
        public static KeyPair FromAccountId(string accountId)
        {
            byte[] decoded = StrKey.DecodeStellarAccountId(accountId);
            return FromPublicKey(decoded);
        }

        /// <summary>
        ///     Creates a new Stellar keypair from a 32 byte address.
        /// </summary>
        /// <param name="publicKey">publicKey The 32 byte public key.</param>
        /// <returns>
        ///     <see cref="KeyPair" />
        /// </returns>
        public static KeyPair FromPublicKey(byte[] publicKey)
        {
            return new KeyPair(publicKey);
        }

        /// <summary>
        ///     Generates a random Stellar keypair.
        /// </summary>
        /// <returns>a random Stellar keypair</returns>
        public static KeyPair Random()
        {
            byte[] b = new byte[32];

            using (RNGCryptoServiceProvider rngCrypto = new RNGCryptoServiceProvider())
            {
                rngCrypto.GetBytes(b);
            }

            return FromSecretSeed(b);
        }

        /// <summary>
        ///     Sign the provided data with the keypair's private key.
        /// </summary>
        /// <param name="data">The data to sign.</param>
        /// <returns>signed bytes, null if the private key for this keypair is null.</returns>
        public byte[] Sign(byte[] data)
        {
            if (PrivateKey == null)
            {
                throw new Exception(
                    "KeyPair does not contain secret key. Use KeyPair.fromSecretSeed method to create a new KeyPair with a secret key.");
            }

            return Ed25519.Sign(data, PrivateKey);
        }

        /// <summary>
        ///     Sign a message and return an XDR Decorated Signature
        /// </summary>
        /// <param name="message"></param>
        /// <returns>
        ///     <see cref="DecoratedSignature" />
        /// </returns>
        public DecoratedSignature SignDecorated(byte[] message)
        {
            byte[] rawSig = Sign(message);

            return new DecoratedSignature
            {
                Hint = new SignatureHint(SignatureHint.InnerValue),
                Signature = new Signature(rawSig)
            };
        }

        /// <summary>
        ///     Verify the provided data and signature match this keypair's public key.
        /// </summary>
        /// <param name="data">The data that was signed.</param>
        /// <param name="signature">The signature.</param>
        /// <returns>True if they match, false otherwise.</returns>
        public bool Verify(byte[] data, byte[] signature)
        {
            bool result = false;

            try
            {
                result = Ed25519.Verify(signature, data, PublicKey);
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}