using IotSupplyStore.Models;

namespace IotSupplyStore.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);
    }
}
