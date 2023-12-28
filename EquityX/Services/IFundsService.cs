namespace EquityX.Services
{
    /// <summary>
    /// This service will allow the user to withdraw and add money to their account
    /// </summary>
    public interface IFundsService
    {
        /// <summary>
        /// This will add funds to the users account
        /// </summary>
        /// <param name="amount"></param>
        /// <returns>Reutns true if succesful otherwise it returns a false</returns>
        public Task<bool> AddFunds(decimal amount);


        /// <summary>
        /// Will remove funds from the user account 
        /// </summary>
        /// <param name="amount"></param>
        /// <returns>Returns true if succesful, otherwisw returns false</returns>
        public Task<bool> WithDrawFunds(decimal amount);

        /// <summary>
        /// ??? Display funds??? 
    }
}
