using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.Shared.Const;

namespace TMS.WebAPP.Controllers
{
    public class ItemTypeController : TMSBaseController
    {
        // GET: ItemType
        public ActionResult List()
        {
            SetActiveFunction(FunctionConst.OrderDataSetupUrl);
            SetActiveFunctionDataSetup(FunctionConst.ItemTypeOrderDataSetupUrl);

            return View();
        }
    }
}