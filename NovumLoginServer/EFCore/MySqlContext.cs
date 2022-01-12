using Microsoft.EntityFrameworkCore;
using NovumLoginServer.DBModels;

namespace NovumLoginServer.EFCore;

public class MySqlContext : DbContext
{

    private static MySqlContext _instance;
    
    public virtual DbSet<Users?> Users { get; set; }
    public virtual DbSet<Sessions?> Sessions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL("server=localhost;database=ffxiv_server;user=root;password=");
    }


    public static MySqlContext Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MySqlContext();
            }

            return _instance;
        } 
    }
}