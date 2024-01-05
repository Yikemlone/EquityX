namespace EquityX.Services
{
    /// <summary>
    /// This service will allow the user to withdraw and add money to their account
    /// </summary>
    public interface IFundsService
    {
        /// <summary>
        /// This will simulate adding funds to the user account by validating with the user's bank
        /// </summary>
        /// <param name="amount"></param>
        /// <returns>bool</returns>
        public Task<bool> ValidateFundsFromBank(decimal amount);


        /// <summary>
        /// Will validate the amount to withdraw and then withdraw the funds from the user's account to their bank
        /// </summary>
        /// <param name="amountToWithdraw"></param>
        /// <param name="availableFunds"></param>  
        /// <returns>bool</returns>
        public Task<bool> WithdrawFunds(decimal amountToWithdraw, decimal availableFunds);
    }
}
