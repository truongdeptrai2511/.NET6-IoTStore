using IotSupplyStore.Models;

namespace IotSupplyStore.Repository.IRepository
{
    public interface IProductOrderRepository : IRepository<ProductOrder>
    {
        void Update(ProductOrder productOrder);
    }
}
