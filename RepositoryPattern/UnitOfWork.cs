using IotSupplyStore.Repository.IRepository;
using IotSupplyStore.DataAccess;

namespace IotSupplyStore.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ApplicationUser = new ApplicationUserRepository(_db);
            Product = new ProductRepository(_db);
            Category = new CategoryRepository(_db);
            Order = new OrderRepository(_db);
            Supplier = new SupplierRepository(_db);
            ProductOrder = new ProductOrderRepository(_db);
            EmployeeRequest = new EmployeeRequestRepository(_db);
            OrderStatus = new OrderStatusRepository(_db);
        }
        public IProductRepository Product { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IOrderRepository Order { get; private set; }
        public ISupplierRepository Supplier { get; private set; }
        public IProductOrderRepository ProductOrder { get; private set; }
        public IOrderStatusRepository OrderStatus { get; private set; }
        public IEmployeeRequestRepository EmployeeRequest { get; private set; }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}