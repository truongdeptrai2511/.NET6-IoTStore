namespace IotSupplyStore.Utility
{
    public class SD
    {
        public const string Role_Admin = "admin";
        public const string Role_Employee = "employee";
        public const string Role_Shipper = "shipper";
        public const string Role_Customer = "customer";

        public const string Policy_SuperAdmin = "Super Admin";
        public const string Policy_AccountManager = "Account Manager";
        public const string Policy_CategoryManager = "Category Manager";
        public const string Policy_ProductManager = "Product Manager";
        public const string Policy_SupplierManager = "SupplierManager";
        public const string Policy_OrderProcess = "Order Process";
        public const string Policy_EditProfile = "Edit Profile";
        public const string Policy_MakeOrder = "Make Order";

        public const string Status_Pending = "Pending";
        public const string Status_Approved = "Approved";
        public const string Status_InProcess = "InProcess";
        public const string Status_Shipped = "Shipped";
        public const string Status_Cancelled = "Cancelled";
        public const string Status_Refunded = "Refunded";

        public const string Payment_StatusPending = "Pending";
        public const string Payment_StatusApproved = "Approved";
        public const string Payment_StatusDelayedPayment = "DelayedPayment";
        public const string Payment_StatusRejected = "Rejected";
    }
}
