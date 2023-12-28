namespace EquityX.Services
{
    public class TestFundsService : IFundsService
    {
        public Task<bool> AddFunds(decimal amount)
        {

            return Task.FromResult(true);
        }

        public Task<bool> WithDrawFunds(decimal amount)
        {
            return Task.FromResult(true);
        }
    }
}
