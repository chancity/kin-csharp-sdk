using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Kin.Jwt.Exceptions;
using Kin.Shared.Models.MarketPlace;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using ECPoint = Org.BouncyCastle.Math.EC.ECPoint;

namespace Kin.Jwt.Extensions
{
    internal static class JwtSecurityKeyExtension
    {
        public static SecurityKey TryConvertPemToSecurityKey(this JwtKey jwtSecurityKey)
        {
            if (jwtSecurityKey.Key.Contains("PRIVATE"))
            {
                return LoadPrivateKey(jwtSecurityKey.Key);
            }

            if (jwtSecurityKey.Key.Contains("PUBLIC"))
            {
                return LoadPublicKey(jwtSecurityKey.Key);
            }

            throw new ConvertPemException(
                $"Unable to determine whether key {jwtSecurityKey.Algorithm} is private or public: {jwtSecurityKey.Key}");
        }

        private static SecurityKey LoadPrivateKey(string privateKeyPem)
        {
            using (StringReader reader = new StringReader(privateKeyPem))
            {
                PemReader pem = new PemReader(reader);
                AsymmetricCipherKeyPair o = (AsymmetricCipherKeyPair) pem.ReadObject();

                if (o.Private is RsaPrivateCrtKeyParameters rsaPrivateCrtKeyParameters)
                {
                    return new RsaSecurityKey(DotNetUtilities.ToRSA(rsaPrivateCrtKeyParameters));
                }

                if (o.Private is ECPrivateKeyParameters eCPrivateKeyParameters)
                {
                    byte[] privateKey = eCPrivateKeyParameters.D.ToByteArrayUnsigned();
                    return new ECDsaSecurityKey(LoadPrivateKey(privateKey));
                }

                return null;
            }
        }

        private static ECDsa LoadPrivateKey(byte[] key)
        {
            BigInteger privKeyInt = new BigInteger(+1, key);
            X9ECParameters parameters = SecNamedCurves.GetByName("secp256k1");
            ECPoint ecPoint = parameters.G.Multiply(privKeyInt);
            byte[] privKeyX = ecPoint.Normalize().XCoord.ToBigInteger().ToByteArrayUnsigned();
            byte[] privKeyY = ecPoint.Normalize().YCoord.ToBigInteger().ToByteArrayUnsigned();

            return ECDsa.Create(new ECParameters
            {
                Curve = ECCurve.CreateFromFriendlyName("secp256k1"),
                D = privKeyInt.ToByteArrayUnsigned(),
                Q = new System.Security.Cryptography.ECPoint
                {
                    X = privKeyX,
                    Y = privKeyY
                }
            });
        }

        private static SecurityKey LoadPublicKey(this string publicKeyPem)
        {
            using (StringReader reader = new StringReader(publicKeyPem))
            {
                PemReader pem = new PemReader(reader);
                AsymmetricKeyParameter o = (AsymmetricKeyParameter) pem.ReadObject();

                if (o is RsaKeyParameters rsaKeyParameters)
                {
                    return new RsaSecurityKey(DotNetUtilities.ToRSA(rsaKeyParameters));
                }

                if (o is ECPublicKeyParameters eCPublicKeyParameters)
                {
                    byte[] publicKey = eCPublicKeyParameters.Q.GetEncoded();
                    return new ECDsaSecurityKey(LoadPublicKey(publicKey));
                }

                return null;
            }
        }

        private static ECDsa LoadPublicKey(byte[] key)
        {
            byte[] pubKeyX = key.Skip(1).Take(32).ToArray();
            byte[] pubKeyY = key.Skip(33).ToArray();

            return ECDsa.Create(new ECParameters
            {
                Curve = ECCurve.CreateFromFriendlyName("secp256k1"),
                Q = new System.Security.Cryptography.ECPoint
                {
                    X = pubKeyX,
                    Y = pubKeyY
                }
            });
        }
    }
}