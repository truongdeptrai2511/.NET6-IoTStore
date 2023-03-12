using System.ComponentModel.DataAnnotations.Schema;

namespace IotSupplyStore.Models.UpsertModel
{
    public class CategoryUpsert
    {
        public string C_Name { get; set; }
        public string C_Home { get; set; }
        public string C_Icon { get; set; }
    }
}
