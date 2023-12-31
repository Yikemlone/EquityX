namespace EquityX.Services
{
    public class TestFundsService : IFundsService
    {
        public Task<bool> ValidateFundsFromBank(decimal amount)
        {
            return Task.FromResult(true);
        }

        public Task<bool> WithDrawFunds(decimal amountToWithDraw, decimal availableFunds)
        {
            return Task.FromResult(true);
        }
    }
}
