using IotSupplyStore.Models;

namespace IotSupplyStore.Repository.IRepository
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Update(Order order);
    }
}
