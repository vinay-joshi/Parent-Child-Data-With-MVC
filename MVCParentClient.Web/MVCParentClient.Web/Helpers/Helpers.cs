using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCParentClient.Model;
using MVCParentClient.Model.Interface;
using MVCParentClient.Web.ViewModels;

namespace MVCParentClient.Web.Helpers
{
    public static class Helpers
    {
        public static SalesOrderViewModel CreateSalesOrderViewModelFromSalesOrder(SalesOrder salesOrder)
        {
            var salesOrderViewModel = new SalesOrderViewModel()
                                      {
                                          PONumber = salesOrder.PONumber,
                                          CustomerName = salesOrder.CustomerName,
                                          SalesOrderId = salesOrder.SalesOrderId,
                                          ObjectState = ObjectState.Unchanged,
                                          RowVersion =  salesOrder.RowVersion
                                      };
            foreach (var salesOrderItem in salesOrder.SalesOrderItems)
            {
                var salesOrderItemViewModel = new SalesOrderItemViewModel()
                                              {
                                                  SalesOrderId = salesOrder.SalesOrderId,
                                                  SalesOrderItemId = salesOrderItem.SalesOrderItemId,
                                                  ProductCode = salesOrderItem.ProductCode,
                                                  Quantity = salesOrderItem.Quantity,
                                                  UnitPrice = salesOrderItem.UnitPrice,
                                                  ObjectState = ObjectState.Unchanged
                                              };
                salesOrderViewModel.SalesOrderItems.Add(salesOrderItemViewModel);
            }

            return salesOrderViewModel;
        }

        public static SalesOrder CreateSalesOrderFromSalesOrderViewModel(SalesOrderViewModel salesOrderViewModel)
        {
            var salesOrder = new SalesOrder()
                             {
                                 CustomerName = salesOrderViewModel.CustomerName,
                                 PONumber = salesOrderViewModel.PONumber,
                                 SalesOrderId = salesOrderViewModel.SalesOrderId,
                                 ObjectState = salesOrderViewModel.ObjectState,
                                 RowVersion = salesOrderViewModel.RowVersion
                             };

            int temporarySalesOrderItemId = -1;

            foreach (var salesOrderItemViewModel in salesOrderViewModel.SalesOrderItems)
            {
                var salesOrderItem = new SalesOrderItem();                
                salesOrderItem.SalesOrderId = salesOrderViewModel.SalesOrderId;
                salesOrderItem.ProductCode = salesOrderItemViewModel.ProductCode;
                salesOrderItem.Quantity = salesOrderItemViewModel.Quantity;
                salesOrderItem.UnitPrice = salesOrderItemViewModel.UnitPrice;
                salesOrderItem.ObjectState = salesOrderItemViewModel.ObjectState;
                if (salesOrderItemViewModel.ObjectState != ObjectState.Added)
                    salesOrderItem.SalesOrderItemId = salesOrderItemViewModel.SalesOrderItemId;
                else
                {
                    salesOrderItem.SalesOrderItemId = temporarySalesOrderItemId;
                    temporarySalesOrderItemId --;
                }
                salesOrder.SalesOrderItems.Add(salesOrderItem);
            }

            return salesOrder;
        }

        public static string GetMessageToClient(ObjectState objectState, string customerName)
        {
            string messageToClient = string.Empty;
            switch (objectState)
            {
                case ObjectState.Added:
                    messageToClient = string.Format("A sales Order for {0} has been added to the data base", customerName);
                    break;
                case ObjectState.Modified:
                    messageToClient = string.Format("The customer name {0} sales order is updated", customerName);
                    break;
                default:
                    break;
            }
            return messageToClient;
        }
    }
}