﻿namespace Kin.Stellar.Sdk
{
    public interface ITransactionBuilderAccount
    {
        KeyPair KeyPair { get; }
        long SequenceNumber { get; }

        /// <summary>
        ///     Returns sequence number incremented by one, but does not increment internal counter.
        /// </summary>
        long IncrementedSequenceNumber { get; }

        /// <summary>
        ///     Increments sequence number in this object by one.
        /// </summary>
        void IncrementSequenceNumber();
    }
}