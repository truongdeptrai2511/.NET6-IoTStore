using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotSupplyStore.Models
{
    public class DetailProduct
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public float P_Price { get; set; }
        public int P_Warranty { get; set; }
        public string P_TitleSeo { get; set; }
        public string P_KeywordSeo { get; set; }
        public string P_Description { get; set; }
        public string P_Content { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
