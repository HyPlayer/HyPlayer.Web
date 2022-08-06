using HyPlayer.Web.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace HyPlayer.Web.DbContexts;

public class SqliteDbContext : DbContext
{
    public DbSet<User>? Users { get; set; }

    public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options)
    {
        
    }
}