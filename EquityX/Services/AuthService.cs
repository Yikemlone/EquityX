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
            //await Task.Delay(3000); // Simulating the validation of the auth key
            var authKey = Preferences.Default.Get(AUTH_STATE, false);
            return true;
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

            Preferences.Default.Set(AUTH_STATE, true);
            Preferences.Default.Set(USER_ID, user.ID);

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
            
            _context.Users
                .Add(new User
                {
                    Name = name,
                    Username = username,
                    Password = encryptedPassword,
                    PortfolioValue = 0,
                    AvailableFunds = 0
                });

            var rowsEffected = await _context.SaveChangesAsync();

            if(rowsEffected <= 0)
            {
                return false;
            }   

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
    }
}
