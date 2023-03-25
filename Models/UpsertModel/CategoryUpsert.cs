using System.ComponentModel.DataAnnotations.Schema;

namespace IotSupplyStore.Models.UpsertModel
{
    public class CategoryUpsert // update or create
    {
        public string CategoryName { get; set; }
    }
}
