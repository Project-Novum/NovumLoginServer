using Microsoft.EntityFrameworkCore;
using NovumLoginServer.DBModels;

namespace NovumLoginServer.EFCore;

public class MySqlContext : DbContext
{

    public virtual DbSet<Users?> Users { get; set; }

    public virtual DbSet<Sessions?> Sessions { get; set; }

    public MySqlContext(DbContextOptions<MySqlContext> options) : base(options)
    {
    }

    /// <summary>
    ///     Override anything needed here for migrations
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}