using IotSupplyStore.Models;
using IotSupplyStore.Repository.IRepository;
using IotSupplyStore.DataAccess;

namespace IotSupplyStore.Repository
{
    public class ProductOrderRepository : Repository<ProductOrder>, IProductOrderRepository
    {
        private ApplicationDbContext _db;
        public ProductOrderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ProductOrder productOrders)
        {
            _db.ProductOrders.Update(productOrders);
        }
    }
}
