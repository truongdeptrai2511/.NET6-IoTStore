using IotSupplyStore.Models;
using IotSupplyStore.Repository.IRepository;
using IotSupplyStore.DataAccess;

namespace IotSupplyStore.Repository
{
    public class OrderStatusRepository : Repository<OrderStatus>, IOrderStatusRepository
    {
        private ApplicationDbContext _db;
        public OrderStatusRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(OrderStatus orderStatus)
        {
            _db.OrderStatus.Update(orderStatus);
        }
    }
}
