using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bogus;
namespace IotSupplyStore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public int UserId { get; set; }
        public string P_Code { get; set; }
        public string P_Status { get; set; }
        public int P_Quantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public ICollection<DetailProduct> DetailProduct { get; set; }
        [ForeignKey("SupplierId")]
        public Suppliers Suppliers { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
    public static class ProductFakeApi
    {
        public static List<Product> GenerateProducts(int count)
        {
            var products = new Faker<Product>()
                .RuleFor(p => p.Id, f => f.IndexGlobal + 1)
                .RuleFor(p => p.CategoryId, f => f.Random.Number(1, 10))
                .RuleFor(p => p.SupplierId, f => f.Random.Number(1, 20))
                .RuleFor(p => p.UserId, f => f.Random.Number(1, 50))
                .RuleFor(p => p.DetailProductId, f => f.Random.Number(1, 100))
                .RuleFor(p => p.P_Code, f => f.Commerce.Product())
                .RuleFor(p => p.P_Status, f => f.PickRandom("In Stock", "Out of Stock"))
                .RuleFor(p => p.P_Quantity, f => f.Random.Number(1, 100))
                .RuleFor(p => p.CreatedAt, f => f.Date.Past())
                .RuleFor(p => p.UpdatedAt, f => f.Date.Recent())
                .RuleFor(p => p.DetailProduct, f => new DetailProduct())
                .RuleFor(p => p.Suppliers, f => new Suppliers())
                .RuleFor(p => p.Category, f => new Category())
                .RuleFor(p => p.Orders, f => new List<Order>());

            return products.Generate(count);
        }

    }

}
