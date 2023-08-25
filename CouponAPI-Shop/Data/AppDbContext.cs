using Microsoft.EntityFrameworkCore;


namespace CouponAPI.Data
{
    public class AppDbContext : DbContext
    {
        
        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=coupons.db");
        }

        
}
}