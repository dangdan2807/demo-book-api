using Microsoft.EntityFrameworkCore;

namespace BookApi_MySQL.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Book>? Books { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<TokenResponse>? TokenResponses { get; set; }
        public DbSet<UserGroup>? UserGroups { get; set; }
        public DbSet<Credential>? Credentials { get; set; }
        public DbSet<Role>? Roles { get; set; }
    }
}
