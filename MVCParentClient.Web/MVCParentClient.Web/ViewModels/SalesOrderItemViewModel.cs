using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MVCParentClient.Model.Interface;

namespace MVCParentClient.Web.ViewModels
{
    public class SalesOrderItemViewModel :IObjectWithState
    {
        public int SalesOrderItemId { get; set; }

        [Required(ErrorMessage = "Server:PONumber is required")]
        [MaxLength(15, ErrorMessage = "Server : PONumber must be 15 Charcter sorter")]
        [RegularExpression(@"^[A-Za-z]+$" ,ErrorMessage = "Server:PONumber must be latters only")]
        public string ProductCode { get; set; }

        [Required(ErrorMessage = "Server: Quantity Required")]
        [Range(1,1000000 ,ErrorMessage = "Server:Quantity Range between 1 to 1000000")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Server :Unit Price is Required!!")]
        [Range(1,100000 ,ErrorMessage = "Server: Range of Unit price between 1, 100000")]
        public decimal UnitPrice { get; set; }

        public int SalesOrderId { get; set; }
        public ObjectState ObjectState { get; set; }       
    }
}