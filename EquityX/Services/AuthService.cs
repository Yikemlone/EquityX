using EquityX.Context;
using EquityX.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace EquityX.Services
{
    public class AuthService : IAuthService
    {
        private const string AUTH_STATE = "AUTH_KEY";
        private const string USER_ID = "USER_ID";

        private readonly EquityXDbContext _context;

        public AuthService(EquityXDbContext context)
        {
            _context = context;
        }   

        public async Task<bool> IsAuthenticated()
        {
            await Task.Delay(1000);
            var authKey = Preferences.Default.Get(AUTH_STATE, false);
            return authKey;
        }

        public async Task<bool> Login(string username, string password)
        {
            string encryptedPassword = HashPassword(password);

            var user = await _context.Users
                .Where(u => u.Username == username && u.Password == encryptedPassword)
                .Select(e => e)
                .FirstOrDefaultAsync();

            if (user == null) 
            { 
                return false;
            }

            // Set the auth key and user id for the session
            UpdateUserSession(true, user.ID);

            return true;
        }

        public void Logout()
        {
            Preferences.Default.Remove(AUTH_STATE);
            Preferences.Default.Remove(USER_ID);
        }

        public async Task<bool> Register(string name, string password, string username)
        {
            string encryptedPassword = HashPassword(password);
            
            var user = new User
            {
                Name = name,
                Username = username,
                Password = encryptedPassword,
                PortfolioValue = 0,
                AvailableFunds = 0
            };
            
            await _context.AddAsync(user);
            var rowsEffected = await _context.SaveChangesAsync();

            if (rowsEffected <= 0)
            {
                return false;
            }

            // Set the auth key and user id for the session
            UpdateUserSession(true, user.ID);

            return true;
        }

        public string HashPassword(string password)
        {
            string encryptedPassword = string.Empty;
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            using (SHA512 shaM = SHA512.Create())
            {
                byte[] hashedPasswordBytes = shaM.ComputeHash(passwordBytes);
                encryptedPassword = Convert.ToBase64String(hashedPasswordBytes);
            }

            return encryptedPassword;
        }

        public void UpdateUserSession(bool isAuthed, int userID)
        {
            Preferences.Default.Remove(AUTH_STATE);
            Preferences.Default.Remove(USER_ID);

            Preferences.Default.Set(AUTH_STATE, isAuthed);
            Preferences.Default.Set(USER_ID, userID);
        }
    }
}
