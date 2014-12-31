using System.Collections.Generic;
using MVCParentClient.Model;

namespace MVCParentClient.DataLayer.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MVCParentClient.DataLayer.SalesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true; 
            
        }

        protected override void Seed(MVCParentClient.DataLayer.SalesContext context)
        {
            context.SalesOrders.AddOrUpdate(so => so.CustomerName,
                new SalesOrder
                {
                    CustomerName = "Vinay Joshi",
                    PONumber = "12345",
                    SalesOrderItems =
                    {
                        new SalesOrderItem()
                        {
                            ProductCode = "VJ123",
                            Quantity = 12,
                            UnitPrice = 1.23m
                        },
                        new SalesOrderItem()
                        {
                            ProductCode = "VJ444",
                              Quantity = 12,
                            UnitPrice = 1.23m
                        }
                    }
                },
                new SalesOrder()
                {
                    CustomerName = "Gaurav Uniyal"
                    ,PONumber = "54321"
                }
                ,new SalesOrder()
                 {
                     CustomerName = "Harsh Yadav"                     
                 });
        }
    }
}
