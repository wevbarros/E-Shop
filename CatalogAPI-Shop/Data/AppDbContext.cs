using Microsoft.EntityFrameworkCore;
namespace Card.Data
{

    public class AppDbContext : DbContext
    {
        public DbSet<Card.Models.Card> Cards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=card.db");
        }
    }
}