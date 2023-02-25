using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMS.WebAPP.Controllers
{
    public class CommonController : TMSBaseController
    {
        // GET: Common

        //page not found
        public ActionResult PageNotFound()
        {
            this.Response.StatusCode = 404;
            this.Response.TrySkipIisCustomErrors = true;
            this.Response.ContentType = "text/html";

            return View();
        }
    }
}