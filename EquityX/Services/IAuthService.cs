namespace EquityX.Services
{
    /// <summary>
    /// This service will handle the authentication of the user for login and registration
    /// </summary>
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

        /// <summary>
        /// Hashes the password for storage in the database
        /// </summary>
        /// <param name="password"></param>
        /// <returns>string</returns>
        public string HashPassword(string password);
    }
}
