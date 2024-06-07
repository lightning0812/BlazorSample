using Blazor_SQLite.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Blazor_SQLite.Data
{
    public class TestDbContext : DbContext
    {
        public DbSet<Weapon> Weapons { get; set; }

        private readonly IWebHostEnvironment _env;

        public TestDbContext(IWebHostEnvironment env)
        {
            _env = env;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var filePath = Path.Combine(_env.WebRootPath, "test.db");
            var connectionString = new SqliteConnectionStringBuilder { DataSource = filePath }.ToString();
            optionsBuilder.UseSqlite(new SqliteConnection(connectionString));
        }
    }
}
