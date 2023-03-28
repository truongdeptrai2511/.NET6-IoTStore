using IotSupplyStore.Models;

namespace IotSupplyStore.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
    }
}
