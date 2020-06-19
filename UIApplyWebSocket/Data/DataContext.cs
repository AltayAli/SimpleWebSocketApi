using Microsoft.EntityFrameworkCore;
using WebSocketrealize.Models;

namespace CheckUserInDatabase.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {}
        public DbSet<User> Users { get; set; }
        public DbSet<Platform> Platforms { get; set; }
    }
}
