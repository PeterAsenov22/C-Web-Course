namespace ChushkaWebApp.Data
{
    using Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ChushkaDbContext : IdentityDbContext<ChushkaUser>
    {
        public ChushkaDbContext(DbContextOptions<ChushkaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }
    }
}
