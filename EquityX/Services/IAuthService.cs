namespace EquityX.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Will check if the user is authenticated
        /// </summary>
        /// <returns>bool</returns>
        public Task<bool> IsAuthenticated();

        /// <summary>
        /// Will validate the user login and return true if successful
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>bool</returns>
        public Task<bool> Login(string username, string password);

        /// <summary>
        /// Will logout the user
        /// </summary>
        public void Logout();

        /// <summary>
        /// Will register the user and return true if successful
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <returns>bool</returns>
        public Task<bool> Register(string username, string password, string email);
    }
}
