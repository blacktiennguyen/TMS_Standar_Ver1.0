using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.Shared.Const;

namespace TMS.WebAPP.Controllers
{
    public class VehicleDataSetupController : TMSBaseController
    {
        // GET: OrderDataSetup
        public ActionResult Index()
        {
            SetActiveFunction(FunctionConst.VehicleDataSetupUrl);

            // Load List Menu Data setup -- Load default first
            return Redirect(FunctionConst.VehicleTypeDataSetupUrl);
        }
    }
}