using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Kin.Jwt.Models;
using Microsoft.IdentityModel.Tokens;

namespace Kin.Jwt
{
    public class JwtProvider
    {
        private static readonly Random Random;
        private readonly Dictionary<string, JwtSecurityKey> _keys;
        private readonly string _kinApplicationId;
        private readonly TokenValidationParameters _validationParameters;

        static JwtProvider()
        {
            Random = new Random();
            AsymmetricSignatureProvider.DefaultMinimumAsymmetricKeySizeInBitsForSigningMap["RS512"] = 1024;
        }

        public JwtProvider(string applicationId, Dictionary<string, JwtSecurityKey> keys)
        {
            if (string.IsNullOrEmpty(applicationId))
            {
                throw new ArgumentNullException($"{nameof(applicationId)}");
            }

            if (keys.Count == 0)
            {
                throw new ArgumentException($"{nameof(keys)} needs to contain at least one object");
            }

            _kinApplicationId = applicationId;
            _keys = keys;

            _validationParameters = new TokenValidationParameters
            {
                ValidIssuer = _kinApplicationId,
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidateTokenReplay = true,
                ValidateActor = true,
                IssuerSigningKeys = _keys.Values.Select(x => x.SecurityKey)
            };
        }

        public string GenerateJwtToken(string subject, params KinJwtPayload[] payloads)
        {
            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentNullException($"{nameof(subject)}");
            }

            if (payloads.Length == 0)
            {
                throw new ArgumentException($"{nameof(payloads)} needs to contain at least one object");
            }

            JwtHeader header = CreateJwtHeader();
            JwtPayload payload = CreateJwtPayload(subject, payloads);

            JwtSecurityToken token = new JwtSecurityToken(header, payload);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            return handler.WriteToken(token);
        }

        private JwtHeader CreateJwtHeader()
        {
            KeyValuePair<string, JwtSecurityKey> jwtSecurityKey = GetRandomJwtKey();

            SigningCredentials signingCredentials =
                new SigningCredentials(jwtSecurityKey.Value.SecurityKey, jwtSecurityKey.Value.Algorithm);
            JwtHeader header = new JwtHeader(signingCredentials);

            return header;
        }

        private JwtPayload CreateJwtPayload(string subject,
            params KinJwtPayload[] kinJwtPayloads)
        {
            DateTime currentTime = DateTime.UtcNow;

            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, subject)
            };

            JwtPayload payload = new JwtPayload(
                _kinApplicationId,
                null,
                claims,
                null,
                currentTime.AddHours(6),
                currentTime);

            foreach (KinJwtPayload kinJwtPayload in kinJwtPayloads)
            {
                payload.Add(kinJwtPayload.Name, kinJwtPayload.Data);
            }

            return payload;
        }

        private KeyValuePair<string, JwtSecurityKey> GetRandomJwtKey()
        {
            string key;

            lock (Random) // synchronize
            {
                key = _keys.Keys.ElementAt(Random.Next(0, _keys.Count));
            }

            JwtSecurityKey value = _keys[key];

            if (string.IsNullOrEmpty(value.SecurityKey.KeyId))
            {
                value.SecurityKey.KeyId = key;
            }

            return new KeyValuePair<string, JwtSecurityKey>(key, value);
        }

        public SecurityToken ValidateJwtToken(string jwt)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            // ClaimsPrincipal claimsPrincipal =
            handler.ValidateToken(jwt, _validationParameters, out SecurityToken validatedToken);


            return validatedToken;
        }
    }
}