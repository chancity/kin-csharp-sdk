namespace Kin.BlockChain.Exceptions
{
    public class NoKinAssetException : BlockChainException
    {
        public NoKinAssetException(string message) : base(message) { }
    }
}