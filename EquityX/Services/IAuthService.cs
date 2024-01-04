namespace EquityX.Services
{
    public interface IAuthService
    {
        public Task<bool> IsAuthenticated();
        public Task<bool> Login(string username, string password);
        public void Logout();
        public Task<bool> Register(string username, string password, string email);
    }
}
