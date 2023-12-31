namespace EquityX.Services
{
    public class FundsService : IFundsService
    {
        public Task<bool> ValidateFundsFromBank(decimal amount)
        {
            // This will simulate validating funds from the user's bank
            return Task.FromResult(true);
        }

        public Task<bool> WithDrawFunds(decimal amountToWithDraw, decimal availableFunds)
        {
            if(amountToWithDraw > availableFunds || amountToWithDraw <= 0)
            {
                return Task.FromResult(false);
            }
            else
            {
                // Simulate withdrawing funds from the user's account to send to their bank of choice
                return Task.FromResult(true);
            }
        }
    }
}
