using Microsoft.EntityFrameworkCore;
using WebProject.Data;

namespace WebProject.Areas.Lab10.Data
{
    public class GameContext : DbContext
    {
        public GameContext(DbContextOptions<GameContext> options)
        : base(options)
        {
        }
        public DbSet<Game> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=GameDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
