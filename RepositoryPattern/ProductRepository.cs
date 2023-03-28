using IotSupplyStore.Models;
using IotSupplyStore.Repository.IRepository;
using IotSupplyStore.DataAccess;

namespace IotSupplyStore.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Product product)
        {
            _db.Products.Update(product);
        }
    }
}
