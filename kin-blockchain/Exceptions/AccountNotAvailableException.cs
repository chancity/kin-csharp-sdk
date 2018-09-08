namespace Kin.BlockChain.Exceptions
{
    public class AccountNotAvailableException : BlockChainException
    {
        public AccountNotAvailableException(string message) : base(message) { }
    }
}