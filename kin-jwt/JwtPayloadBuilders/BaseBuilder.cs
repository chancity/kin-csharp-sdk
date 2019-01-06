using System;
using System.Collections.Generic;
using Kin.Jwt.Models;
using Newtonsoft.Json;

namespace Kin.Jwt.JwtPayloadBuilders
{
    public abstract class BaseBuilder
    {
        private readonly int _expectedPayloadsCount;
        private readonly Dictionary<string, object> _payloads;
        private readonly JwtProvider _provider;
        private readonly string _subject;

        public string Jwt => ToJwt();
        public string Payload => ToPayload();
        protected BaseBuilder(JwtProvider provider, string subject, int expectedPayloadsCount)
        {
            _provider = provider;
            _subject = subject;
            _expectedPayloadsCount = expectedPayloadsCount;
            _payloads = new Dictionary<string,object>();
        }

        protected void AddPayload(KinJwtPayload payload)
        {
            _payloads.Add(payload.Name,payload.Data);

            if (_expectedPayloadsCount != 0 && _payloads.Count > _expectedPayloadsCount)
            {
                throw new ArgumentException(
                    $"{_subject} requires only {_expectedPayloadsCount + 1} and got {_payloads.Count}");
            }
        }

        private string ToJwt()
        {
            if (_provider == null)
                throw new ArgumentNullException($"{nameof(_provider)} is null, use property Payload instead");

            if (_expectedPayloadsCount == 0 || _payloads.Count == _expectedPayloadsCount)
            {
                return _provider.GenerateJwtToken(_subject, _payloads);
            }

            throw new ArgumentException($"{_subject} expected {_expectedPayloadsCount + 1} and got {_payloads.Count}");
        }

        public string ToPayload()
        {
            if (_expectedPayloadsCount == 0 || _payloads.Count == _expectedPayloadsCount)
            {
                return JsonConvert.SerializeObject(_payloads);
            }

            throw new ArgumentException($"{_subject} expected {_expectedPayloadsCount + 1} and got {_payloads.Count}");
        }
    }
}