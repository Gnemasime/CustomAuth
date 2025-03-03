using CustomAuth.Models;
using Microsoft.EntityFrameworkCore;

public class MySqlDbContext : DbContext
{
    public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}
