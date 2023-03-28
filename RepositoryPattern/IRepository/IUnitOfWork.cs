namespace IotSupplyStore.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IApplicationUserRepository ApplicationUser { get; }
        IProductRepository Product { get; }
        ICategoryRepository Category { get; }
        IOrderRepository Order { get; }
        ISupplierRepository Supplier { get; }
        IProductOrderRepository ProductOrder { get; }
        IEmployeeRequestRepository EmployeeRequest { get; }
        IOrderStatusRepository OrderStatus { get; }

        Task Save();
    }
}
