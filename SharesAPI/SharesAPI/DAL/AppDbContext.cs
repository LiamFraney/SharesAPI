using SharesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace SharesAPI.DatabaseAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        //passes options to overloaded base implementation
        {

        }

        public DbSet<Share> Shares { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AquiredShares> AquiredShares {get; set;}
    
    }
}
