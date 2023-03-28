using System.ComponentModel.DataAnnotations.Schema;

namespace IotSupplyStore.Models.UpsertModel
{
    public class ShoppingCartUpsert
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public float Price { get; set; }
    }
}
