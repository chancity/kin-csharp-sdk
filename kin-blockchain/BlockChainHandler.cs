using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Kin.BlockChain.Exceptions;
using Kin.Shared.Models.MarketPlace;
using Kin.Stellar.Sdk;
using Kin.Stellar.Sdk.responses;
using Kin.Stellar.Sdk.xdr;
using Kin.Tooling.Models.Impl;
using Asset = Kin.Stellar.Sdk.Asset;
using Transaction = Kin.Stellar.Sdk.Transaction;

namespace Kin.BlockChain
{
    public class BlockChainHandler
    {
        private readonly string _appId;
        private readonly Asset _kinAsset;
        private readonly Server _server;

        public BlockChainHandler(string horizonUrl, string networkId, string appId, HttpMessageHandler httpMessageHandler = null)
        {
            _kinAsset = new AssetTypeNative();

            _server = new Server(horizonUrl + "/", new HttpClient(new MetricHttpHandler(httpMessageHandler ?? new HttpClientHandler())));
            Network.UsePublicNetwork();
            Network.Use(new Network(networkId));
            _appId = appId; 
        }
        public async Task<Transaction> GetTransaction(KeyPair sourceKeyPair, string destinationAddress, double amount, string marketPlaceOrderId = null)
        {
            KeyPair destinationKeyPair = KeyPair.FromAccountId(destinationAddress);
            AccountResponse sourceAccount = await GetAccount(sourceKeyPair);
            AccountResponse destinationAccount = await GetAccount(destinationKeyPair);
            Transaction response = await GetTransaction(sourceKeyPair, destinationKeyPair, sourceAccount, amount / 100, marketPlaceOrderId).ConfigureAwait(false);
            return response;
        }
        private Task<Transaction> GetTransaction(KeyPair sourceKeyPair, KeyPair destinationKeyPair, AccountResponse sourceAccount, double amount, string marketPlaceOrderId = null)
        {
            PaymentOperation.Builder paymentOperationBuilder =
                new PaymentOperation.Builder(destinationKeyPair, _kinAsset,
                        amount.ToString(CultureInfo.InvariantCulture))
                    .SetSourceAccount(sourceKeyPair);

            PaymentOperation paymentOperation = paymentOperationBuilder.Build();

            Transaction.Builder paymentTransaction =
                new Transaction.Builder(new Account(sourceKeyPair, sourceAccount.SequenceNumber)).AddOperation(
                    paymentOperation);

            string toAppend = string.IsNullOrEmpty(marketPlaceOrderId) ? "p2p" : marketPlaceOrderId;

            paymentTransaction.AddMemo(new MemoText($"1-{_appId}-{toAppend}"));

            Transaction transaction = paymentTransaction.Build();
            transaction.Sign(sourceKeyPair);
            return Task.FromResult(transaction);
        }

        private async Task<SubmitTransactionResponse> SendPaymentOperation(Transaction transaction)
        {
            return await _server.SubmitTransaction(transaction).ConfigureAwait(false);
        }
        public async Task<double> GetKinBalance(KeyPair keyPair)
        {
            AccountResponse accountResponse = await GetAccount(keyPair).ConfigureAwait(false);
            return GetKinBalance(accountResponse);
        }
        private static double GetKinBalance(AccountResponse account)
        {
            foreach (Balance accountBalance in account.Balances)
            {
                if (accountBalance.AssetType.Equals("native"))
                {
                    return double.Parse(accountBalance.BalanceString);
                }
            }

            throw new NoKinAssetException($"{account.KeyPair.Address} doesn't have the native asset");
        }
        private async Task<AccountResponse> GetAccount(KeyPair account)
        {
            AccountResponse accountResponse = await _server.Accounts.Account(account).ConfigureAwait(false);

            if (accountResponse == null)
            {
                throw new AccountNotAvailableException($"Can't retrieve data for account {account.AccountId}");
            }

            return accountResponse;
        }
    }
}