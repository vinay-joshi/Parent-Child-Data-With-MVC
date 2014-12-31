using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCParentClient.DataLayer;
using MVCParentClient.DataLayer.Helpers;
using MVCParentClient.Model;
using MVCParentClient.Model.Interface;
using MVCParentClient.Web.ViewModels;

namespace MVCParentClient.Web.Controllers
{
    public class SalesController : Controller
    {

        private SalesContext _salesContext;

        public SalesController()
        {
             _salesContext = new SalesContext();
        }

        public ActionResult Index()
        {
            return View(_salesContext.SalesOrders.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var salesOrder = _salesContext.SalesOrders.Find(id);
            if (salesOrder == null)
            {
                return HttpNotFound();
            }           
            var salesOrderViewModel = Helpers.Helpers.CreateSalesOrderViewModelFromSalesOrder(salesOrder);
            return View(salesOrderViewModel);
        }

        public ActionResult Create()
        {
            var  salesOrderViewModel = new SalesOrderViewModel {ObjectState = ObjectState.Added};
            return View(salesOrderViewModel);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "SalesOrderId,CustomerName,PONumber")] SalesOrder salesOrder)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _salesContext.SalesOrders.Add(salesOrder);
        //        _salesContext.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(salesOrder);
        //}

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesOrder salesOrder = _salesContext.SalesOrders.Find(id);
            if (salesOrder == null)
            {
                return HttpNotFound();
            }
            var salesOrderViewModel = Helpers.Helpers.CreateSalesOrderViewModelFromSalesOrder(salesOrder);
           // salesOrderViewModel.ObjectState = ObjectState.Unchanged;            
            return View(salesOrderViewModel);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "SalesOrderId,CustomerName,PONumber")] SalesOrder salesOrder)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _salesContext.Entry(salesOrder).State = EntityState.Modified;
        //        _salesContext.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(salesOrder);
        //}

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesOrder salesOrder = _salesContext.SalesOrders.Find(id);
            if (salesOrder == null)
            {
                return HttpNotFound();
            }
            var salesOrderViewModel = Helpers.Helpers.CreateSalesOrderViewModelFromSalesOrder(salesOrder);
            salesOrderViewModel.MessageToClient = "You are about to delte the record";
            salesOrderViewModel.ObjectState = ObjectState.Deleted;
            return View(salesOrderViewModel);
        }

        //[HttpPost,ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    SalesOrder salesOrder = _salesContext.SalesOrders.Find(id);
        //    _salesContext.SalesOrders.Remove(salesOrder);
        //    _salesContext.SaveChanges();
        //    return RedirectToAction("Index");
        //}      

        [HandleModelStateException]
        public JsonResult Save(SalesOrderViewModel salesOrderViewModel)
        {           
            if (!ModelState.IsValid)
            {
                throw new ModelStateException(ModelState);
            }
            var salesOrder = Helpers.Helpers.CreateSalesOrderFromSalesOrderViewModel(salesOrderViewModel);
            _salesContext.SalesOrders.Attach(salesOrder);

            if (salesOrder.ObjectState == ObjectState.Deleted)
            {
                foreach (var salesOrderItemViewModel in salesOrderViewModel.SalesOrderItems)
                {
                    var salesOrderItem = _salesContext.SalesOrderItems.Find(salesOrderItemViewModel.SalesOrderItemId);
                    if (salesOrderItem != null)
                        salesOrderItem.ObjectState = ObjectState.Deleted;
                }
            }
            else
            {
                foreach (int salesOrderItemId in salesOrderViewModel.SalesOrderItemToDelete)
                {
                    var salesOrderItem = _salesContext.SalesOrderItems.Find(salesOrderItemId);
                    if (salesOrderItem != null)
                        salesOrderItem.ObjectState = ObjectState.Deleted;
                }
            } 
                    
            _salesContext.ApplyStateChanges();
            try
            {
                  _salesContext.SaveChanges();
            }
            catch (Exception exception)
            {
                    throw new ModelStateException(exception);               
            }
          

            if (salesOrder.ObjectState == ObjectState.Deleted)           
                return Json(new {newLocation = "/Sales/Index/"});

            string messageToClient = Helpers.Helpers.GetMessageToClient(salesOrderViewModel.ObjectState,
                salesOrderViewModel.CustomerName); 
         
            salesOrderViewModel = Helpers.Helpers.CreateSalesOrderViewModelFromSalesOrder(salesOrder);
            salesOrderViewModel.MessageToClient = messageToClient;
            return Json( new {salesOrderViewModel});
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _salesContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}