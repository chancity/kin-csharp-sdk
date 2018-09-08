using System;

namespace Kin.BlockChain.Exceptions
{
    public class BlockChainException : Exception
    {
        public BlockChainException() { }
        public BlockChainException(string message) : base(message) { }
    }
}