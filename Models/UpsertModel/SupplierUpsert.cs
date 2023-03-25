using System.ComponentModel.DataAnnotations;

namespace IotSupplyStore.Models.UpsertModel
{
    public class SupplierUpsert
    {
        public string SupplierName { get; set; }
        public string SupplierEmail { get; set; }
        public string SupplierPhoneNumber { get; set; }
        public string SupplierFax { get; set; }
    }
}
