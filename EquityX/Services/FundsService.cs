using EquityX.Context;
using EquityX.Models;
using Microsoft.EntityFrameworkCore;

namespace EquityX.Services
{
    public class FundsService : IFundsService
    {
        EquityXDbContext _context;  

        public FundsService(EquityXDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddFunds(decimal amount, int userID)
        {
            User user =  await _context.Users
                .Where(u => u.ID == userID)
                .Select(e => e)
                .FirstOrDefaultAsync();

            user.AvailableFunds += amount;

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected == 0)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> WithdrawFunds(decimal amountToWithDraw, int userID)
        {
            User user = await _context.Users
                .Where(u => u.ID == userID)
                .Select(e => e)
                .FirstOrDefaultAsync();

            if (amountToWithDraw > user.AvailableFunds || amountToWithDraw <= 0)
            {
                return false;
            }

            user.AvailableFunds -= amountToWithDraw;

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected == 0)
            {
                return false;
            }

            return true;
        }
    }
}
