using System;
using System.Collections.Generic;
using Kin.Jwt.Models;

namespace Kin.Jwt.JwtPayloadBuilders
{
    public abstract class BaseBuilder
    {
        private readonly int _expectedPayloadsCount;
        private readonly List<KinJwtPayload> _payloads;
        private readonly JwtProvider _provider;
        private readonly string _subject;

        public string Jwt => Build();

        protected BaseBuilder(JwtProvider provider, string subject, int expectedPayloadsCount)
        {
            _provider = provider;
            _subject = subject;
            _expectedPayloadsCount = expectedPayloadsCount;
            _payloads = new List<KinJwtPayload>();
        }

        protected void AddPayload(KinJwtPayload payload)
        {
            _payloads.Add(payload);

            if (_expectedPayloadsCount != 0 && _payloads.Count > _expectedPayloadsCount)
            {
                throw new ArgumentException(
                    $"{_subject} requires only {_expectedPayloadsCount + 1} and got {_payloads.Count}");
            }
        }

        private string Build()
        {
            if (_expectedPayloadsCount == 0 || _payloads.Count == _expectedPayloadsCount)
            {
                return _provider.GenerateJwtToken(_subject, _payloads.ToArray());
            }

            throw new ArgumentException($"{_subject} expected {_expectedPayloadsCount + 1} and got {_payloads.Count}");
        }

        public override string ToString()
        {
            if (_expectedPayloadsCount == 0 || _payloads.Count == _expectedPayloadsCount)
            {
                return _provider.GenerateJwtToken(_subject, _payloads.ToArray());
            }

            throw new ArgumentException($"{_subject} expected {_expectedPayloadsCount + 1} and got {_payloads.Count}");
        }
    }
}