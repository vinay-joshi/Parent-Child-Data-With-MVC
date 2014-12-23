using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MVCParentClient.Web.ViewModels
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public sealed class HandleModelStateException : FilterAttribute ,IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw  new ArgumentException("FilterContext");
            }

            if (filterContext.Exception != null &&
                typeof (ModelStateException).IsInstanceOfType(filterContext.Exception) &&
                !filterContext.ExceptionHandled)
            {
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.ContentEncoding = Encoding.UTF8;
                filterContext.HttpContext.Response.HeaderEncoding = Encoding.UTF8;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                filterContext.HttpContext.Response.StatusCode = 400;
                filterContext.Result = new ContentResult
                                       {
                                           Content = (filterContext.Exception as ModelStateException).Message,
                                           ContentEncoding = Encoding.UTF8
                                       };
            }
        }
    }
}