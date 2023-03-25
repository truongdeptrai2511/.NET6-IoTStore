namespace IotSupplyStore.Models.ViewModel
{
    public class OrderVM
    {
        public Order Order { get; set; }
        public IEnumerable<ProductOrder> ProductOrders { get; set; }
    }
}
