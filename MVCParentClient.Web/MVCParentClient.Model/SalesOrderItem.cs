using MVCParentClient.Model.Interface;

namespace MVCParentClient.Model
{
    public class SalesOrderItem :IObjectWithState
    {
        public int SalesOrderItemId { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public int SalesOrderId { get; set; }
        public SalesOrder SalesOrder { get; set; }
        public ObjectState ObjectState { get; set; }       
    }
}
