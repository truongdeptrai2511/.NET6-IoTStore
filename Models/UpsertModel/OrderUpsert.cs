using System.ComponentModel.DataAnnotations.Schema;

namespace IotSupplyStore.Models.UpsertModel
{
    public class OrderUpsert
    {
        public int Id { get; set; }

        public int TransactionId { get; set; }
        public int ProductId { get; set; }
        public int Or_Quantity { get; set; }
        public float Or_Price { get; set; }
        public float Or_PriceSale { get; set; }

        public string ApplicationUserId { get; set; }
    }
}
