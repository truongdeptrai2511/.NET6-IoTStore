using IotSupplyStore.Models;

namespace IotSupplyStore.Repository.IRepository
{
    public interface IOrderStatusRepository : IRepository<OrderStatus>
    {
        void Update(OrderStatus orderStatus);
    }
}
