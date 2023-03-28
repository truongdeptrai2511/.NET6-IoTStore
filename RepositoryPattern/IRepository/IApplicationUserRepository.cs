using IotSupplyStore.Models;

namespace IotSupplyStore.Repository.IRepository
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        void Update(ApplicationUser user);
    }
}
