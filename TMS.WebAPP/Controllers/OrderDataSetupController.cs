using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.Shared.Const;

namespace TMS.WebAPP.Controllers
{
    public class OrderDataSetupController : TMSBaseController
    {
        // GET: OrderDataSetup
        public ActionResult Index()
        {
            SetActiveFunction(FunctionConst.OrderDataSetupUrl);

            // Load List Menu Data setup
            return View();
        }
    }
}