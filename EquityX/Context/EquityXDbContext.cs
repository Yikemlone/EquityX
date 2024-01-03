using Microsoft.EntityFrameworkCore;
using EquityX.Models;
using EquityX.Utilities;

namespace EquityX.Context
{
    public class EquityXDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserStockData> UserStockData { get; set; }
        public DbSet<UserWatchlist> UserWatchlist { get; set; }
        public DbSet<StockData> StockData { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            string connectionString = $"Filename={PathDB.GetPath("EquityX.db")}";
            optionsBuilder.UseSqlite(connectionString);
        }
    }
}
