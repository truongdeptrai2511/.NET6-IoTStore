using IotSupplyStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IotSupplyStore.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<DetailProduct> DetailsProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Suppliers> Suppliers { get; set; }
        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<EmployeeRequest> EmployeeRequests { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
