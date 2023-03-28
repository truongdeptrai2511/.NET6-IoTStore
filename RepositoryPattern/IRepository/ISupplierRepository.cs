using IotSupplyStore.Models;

namespace IotSupplyStore.Repository.IRepository
{
    public interface ISupplierRepository : IRepository<Suppliers>
    {
        void Update(Suppliers suppliers);
    }
}
