using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IotSupplyStore.Models.UpsertModel
{
    public class ProductUpsert
    {
        public string ProductName { get; set; }
        [StringLength(100)]
        public string Code { get; set; }
        [StringLength(50)]
        public string Status { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string ImgName { get; set; }
        public string Description { get; set; }

        public int SupplierId { get; set; }
        public int CategoryId { get; set; }
    }
}
