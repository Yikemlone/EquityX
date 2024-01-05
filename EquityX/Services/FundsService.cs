namespace EquityX.Services
{
    public class FundsService : IFundsService
    {
        public async Task<bool> ValidateFundsFromBank(decimal amount)
        {
            // This will simulate validating funds from the user's bank
            await Task.Delay(2000);
            return true;
        }

        public async Task<bool> WithdrawFunds(decimal amountToWithDraw, decimal availableFunds)
        {

            if(amountToWithDraw > availableFunds || amountToWithDraw <= 0)
            {
                return false;
            }

            // Simulate withdrawing funds from the user's account to send to their bank of choice
            await Task.Delay(2000);
            return true;
        }
    }
}
