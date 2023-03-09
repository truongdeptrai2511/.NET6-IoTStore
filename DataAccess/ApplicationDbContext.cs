using IotSupplyStore.Models;
using Microsoft.EntityFrameworkCore;

namespace IotSupplyStore.DataAccess
{
    public class ApplicationDbContext : DbContext /*Lop ke thua DBContext de xu ly các thao tac voi csdl*/
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DetailProduct> DetailsProducts { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Suppliers> Suppliers { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) /*Cau hinh cac quan he giua cac bang trong csdl*/
        {
            /*Product n - n voi order nen su dung HasMany va UsingEntity de dat ten cho bang lien ket giua hai bang nay*/
            modelBuilder.Entity<Product>()
                        .HasMany(p => p.Orders)
                        .WithMany(c => c.Products)
                        .UsingEntity(j => j.ToTable("ProductOrder"));
        }
}
}
