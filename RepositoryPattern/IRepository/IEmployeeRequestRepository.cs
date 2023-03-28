using IotSupplyStore.Models;

namespace IotSupplyStore.Repository.IRepository
{
    public interface IEmployeeRequestRepository : IRepository<EmployeeRequest>
    {
        void Update(EmployeeRequest employeeRequest);
    }
}
