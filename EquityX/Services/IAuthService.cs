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
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="username"></param>
        /// <returns>bool</returns>
        public Task<bool> Register(string name, string password, string username);

        /// <summary>
        /// Hashes the password for storage in the database
        /// </summary>
        /// <param name="password"></param>
        /// <returns>string</returns>
        public string HashPassword(string password);
    
        /// <summary>
        /// Will remove the current auth key and user id from the session if there is one
        /// then add the new auth key and user id to the session
        /// </summary>
        /// <param name="authKey"></param>
        /// <param name="userID"></param>
        public void UpdateUserSession(bool authKey, int userID);
    }
}
