using HyPlayer.Web.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace HyPlayer.Web.DbContexts;

public class SqliteDbContext(DbContextOptions<SqliteDbContext> options) : DbContext(options)
{
    public DbSet<User>? Users { get; set; }
    public DbSet<Release>? Releases { get; set; }
}