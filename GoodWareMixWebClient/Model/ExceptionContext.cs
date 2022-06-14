using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace GoodWareMixWebClient.Model
{
    public class ExceptionContext
    {
        public ActionDescriptor ActionDescriptor { get; set; }
        public ActionResult Result { get; set; }
        public Exception Exception { get; set; }
        public bool ExceptionHandled { get; set; }
    }
}
