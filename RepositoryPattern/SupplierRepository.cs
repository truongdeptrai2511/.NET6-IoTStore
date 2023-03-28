using IotSupplyStore.Models;
using IotSupplyStore.Repository.IRepository;
using IotSupplyStore.DataAccess;

namespace IotSupplyStore.Repository
{
    public class SupplierRepository : Repository<Suppliers>, ISupplierRepository
    {
        private ApplicationDbContext _db;
        public SupplierRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Suppliers suppliers)
        {
            _db.Suppliers.Update(suppliers);
        }
    }
}
