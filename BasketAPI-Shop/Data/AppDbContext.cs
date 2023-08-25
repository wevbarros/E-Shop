using Microsoft.EntityFrameworkCore;

namespace Basket.API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlite("Data Source=app.db");  
}
}