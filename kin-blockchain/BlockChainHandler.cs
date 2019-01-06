using System.Globalization;
using System.Threading.Tasks;
using Kin.BlockChain.Exceptions;
using Kin.Shared.Models.MarketPlace;
using Kin.Stellar.Sdk;
using Kin.Stellar.Sdk.responses;

namespace Kin.BlockChain
{
    public class BlockChainHandler
    {
        private readonly string _appId;
        private readonly Asset _kinAsset;

        private readonly Server _server;
        private readonly string TRUST_NO_LIMIT_VALUE;
        private string MAIN_NETWORK_ISSUER;

        public BlockChainHandler(Config config, string appId)
        {
            _kinAsset = Asset.CreateNonNativeAsset(config.BlockChain.AssetCode,
                KeyPair.FromAccountId(config.BlockChain.AssetIssuer));
            MAIN_NETWORK_ISSUER = config.BlockChain.AssetIssuer;
            TRUST_NO_LIMIT_VALUE = "922337203685.4775807";
            _server = new Server(config.BlockChain.HorizonUrl + "/");
            Network.UsePublicNetwork();
            Network.Use(new Network(config.BlockChain.NetworkPassphrase));
            _appId = appId;
        }

        public async Task<bool> TryUntilActivated(KeyPair account)
        {
            bool activated = false;
            int tries = 10;

            do
            {
                try
                {
                    tries--;
                    activated = await Activate(account).ConfigureAwait(false);

                    if (activated)
                    {
                        break;
                    }
                }
                catch
                {
                    await Task.Delay(3000);

                    if (tries <= 0)
                    {
                        throw;
                    }
                }
            } while (tries > 0);

            return activated;
        }

        public async Task<bool> Activate(KeyPair account)
        {
            AccountResponse accountResponse = await GetAccount(account).ConfigureAwait(false);

            if (!HasKinAsset(accountResponse))
            {
                SubmitTransactionResponse response =
                    await SendAllowKinTrustOperation(account, accountResponse).ConfigureAwait(false);
                return response != null;
            }

            return true;
        }

        private async Task<SubmitTransactionResponse> SendAllowKinTrustOperation(KeyPair account,
            AccountResponse accountResponse)
        {
            ChangeTrustOperation.Builder changeTrustOperationBuilder = new ChangeTrustOperation.Builder(
                (AssetTypeCreditAlphaNum) _kinAsset,
                TRUST_NO_LIMIT_VALUE).SetSourceAccount(account);
            ChangeTrustOperation changeTrustOperation = changeTrustOperationBuilder.Build();

            Transaction.Builder allowKinTrustTransaction =
                new Transaction.Builder(new Account(account, accountResponse.SequenceNumber)).AddOperation(
                    changeTrustOperation);


            Transaction transaction = allowKinTrustTransaction.Build();
            transaction.Sign(account);
            return await _server.SubmitTransaction(transaction).ConfigureAwait(false);
        }

        public async Task<SubmitTransactionResponse> SendPayment(KeyPair sourceKeyPair, string destinationAddress,
            double amount, string marketPlaceOrderId = null)
        {
            KeyPair destinationKeyPair = KeyPair.FromAccountId(destinationAddress);
            AccountResponse sourceAccount = await GetAccount(sourceKeyPair);
            AccountResponse destinationAccount = await GetAccount(destinationKeyPair);

            if (HasKinAsset(sourceAccount, true, amount) && HasKinAsset(destinationAccount))
            {
                SubmitTransactionResponse response =
                    await SendPaymentOperation(sourceKeyPair, destinationKeyPair, sourceAccount, amount,
                        marketPlaceOrderId).ConfigureAwait(false);
                return response;
            }

            return null;
        }

        private async Task<SubmitTransactionResponse> SendPaymentOperation(KeyPair sourceKeyPair,
            KeyPair destinationKeyPair, AccountResponse sourceAccount, double amount, string marketPlaceOrderId = null)
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
            return await _server.SubmitTransaction(transaction).ConfigureAwait(false);
        }

        private static bool HasKinAsset(AccountResponse account, bool checkBalance = false, double amount = 0)
        {
            foreach (Balance accountBalance in account.Balances)
            {
                if (accountBalance.AssetCode != null && accountBalance.AssetCode.Equals("KIN"))
                {
                    if (checkBalance)
                    {
                        if (double.Parse(accountBalance.BalanceString) < amount)
                        {
                            throw new NotEnoughKinException();
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        public async Task<double> GetKinBalance(string accountId)
        {
            AccountResponse accountResponse = await GetAccount(KeyPair.FromAccountId(accountId)).ConfigureAwait(false);
            return GetKinBalance(accountResponse);
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
                if (accountBalance.AssetCode != null && accountBalance.AssetCode.Equals("KIN"))
                {
                    return double.Parse(accountBalance.BalanceString);
                }
            }

            throw new NoKinAssetException($"{account.KeyPair.Address} doesn't have kin asset");
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