using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;
using ShoppingCart.Models;

namespace ShoppingCart.Infrastructure
{
        public class DataContext : IdentityDbContext<AppUser>
        {
                public DataContext(DbContextOptions<DataContext> options) : base(options)
                { }
                public DbSet<Product> Products { get; set; }
                public DbSet<Category> Categories { get; set; }
                public DbSet<ShoppingCart.Models.empdata> empdata { get; set; }

                public DbSet<Orders> Orders { get; set; }

                public DbSet<OrderDetails> OrderDetails { get; set; }
    }
}
