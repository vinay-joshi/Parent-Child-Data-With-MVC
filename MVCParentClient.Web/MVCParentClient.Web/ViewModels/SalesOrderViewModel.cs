using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MVCParentClient.Model;
using MVCParentClient.Model.Interface;

namespace MVCParentClient.Web.ViewModels
{
    public class SalesOrderViewModel : IObjectWithState
    {
        public SalesOrderViewModel()
        {
            SalesOrderItems = new List<SalesOrderItemViewModel>();
            SalesOrderItemToDelete = new List<int>();
        }

        
        public int SalesOrderId { get; set; }

        [Required(ErrorMessage = "Server : You can not create a sales Order with out cusetmer name")]
        [MaxLength(30 ,ErrorMessage = "Server :Customer Name must be 30 charcter")]
        public string CustomerName { get; set; }

        [MaxLength(10, ErrorMessage = "Server :Customer Name must be 10 charcter")]
        public string PONumber { get; set; }
        public string MessageToClient { get; set; }
        public ObjectState ObjectState { get; set; }
        public List<SalesOrderItemViewModel> SalesOrderItems { get; set; }
        public List<int> SalesOrderItemToDelete { get; set; }
        public byte[] RowVersion { get; set; }
    }
}