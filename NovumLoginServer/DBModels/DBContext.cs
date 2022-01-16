using Microsoft.EntityFrameworkCore;

namespace NovumLoginServer.DBModels
{
    public class DBContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("Accounts");
        }
    }
}
