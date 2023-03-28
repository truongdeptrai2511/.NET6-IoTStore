using IotSupplyStore.Models;
using IotSupplyStore.Repository.IRepository;
using IotSupplyStore.DataAccess;

namespace IotSupplyStore.Repository
{
    public class EmployeeRequestRepository : Repository<EmployeeRequest>, IEmployeeRequestRepository
    {
        private ApplicationDbContext _db;
        public EmployeeRequestRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(EmployeeRequest employeeRequest)
        {
            _db.EmployeeRequests.Update(employeeRequest);
        }
    }
}
