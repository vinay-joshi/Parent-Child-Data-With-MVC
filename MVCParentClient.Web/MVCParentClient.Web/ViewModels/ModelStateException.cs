 using System;
using System.Collections.Generic;
using System.Linq;
 using System.Security.Cryptography;
 using System.Text;
 using System.Web;
 using System.Web.Mvc;

namespace MVCParentClient.Web.ViewModels
{
    public class ModelStateException : Exception
    {
        public Dictionary<string,string> Errors { get; set; }

        public ModelStateException(Exception exception)
        {
            string message = (exception.InnerException != null && exception.InnerException.InnerException != null)
                ? exception.InnerException.InnerException.Message : exception.Message;

            Errors = new Dictionary<string, string>();
            Errors.Add(string.Empty, message);
        }

        public ModelStateException(ModelStateDictionary modelState)
        {
            if (modelState == null)
            {
                throw new ArgumentException("ModelState ");
            }
             
            Errors = new Dictionary<string, string>();
            if (!modelState.IsValid)
            {
                StringBuilder errors;
                foreach (KeyValuePair<string, ModelState> state in modelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        errors =new StringBuilder();
                        foreach (var err in state.Value.Errors)
                        {
                            errors.AppendLine(err.ErrorMessage);
                        }
                        Errors.Add(state.Key,errors.ToString());
                    }
                }
            }


        }

        public override string Message
        {
            get
            {
                if (Errors.Count > 0)
                {
                    return string.Join(" | ", Errors.Values.ToArray());
                }
                return null;
            }
        }
    }
}