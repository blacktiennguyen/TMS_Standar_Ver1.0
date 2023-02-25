using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.Core;
using TMS.Core.Domains;
using TMS.Core.Extend;
using TMS.Library.Commons;
using TMS.Service.Customers;
using TMS.Service.FixDataTranslations;
using TMS.Service.FIXs;
using TMS.Service.MasterDatas;
using TMS.Service.MasterDataTranslations;
using TMS.Service.Orders;
using TMS.Service.Users;
using TMS.WebAPP.Framework.Controllers;
using TMS.WebAPP.Models;
using TMS.WebAPP.Models.Order;

namespace TMS.WebAPP.Controllers
{
    public class VehicleTypeController : TMSBaseController
    {
        #region Fields

        private readonly IVehicleTypeService _vehicleTypeService;
        private readonly IMasterDataTranslationService _masterDataTranslationService;

        #endregion Fields

        #region Constructors

        public VehicleTypeController(IVehicleTypeService vehicleTypeService,
            IMasterDataTranslationService masterDataTranslationService)
        {
            this._vehicleTypeService = vehicleTypeService;
            this._masterDataTranslationService = masterDataTranslationService;
        }

        #endregion Constructors

        #region Vehicle Type Load DropDownList

        public JsonResult LoadVehicleTypeForDropDownList()
        {
            var vehicleTypes = new List<DropDownListItemExtend>();

            var itemEmpty = new DropDownListItemExtend();
            itemEmpty.Id = 0;
            itemEmpty.Name = "";
            vehicleTypes.Add(itemEmpty);

            var getVehicleTypes = _vehicleTypeService.GetAlls(CompanyCurrent.Id, CompanyCurrent.TenantId);

            if (getVehicleTypes != null && getVehicleTypes.Count > 0)
            {
                foreach (var obj in getVehicleTypes)
                {
                    var item = new DropDownListItemExtend();

                    var vehicleTranslationName = _masterDataTranslationService.GetName(LanguageCurrent.Id, obj.TranslationId);
                    var vehicleTypeName = obj.Name;

                    if (!string.IsNullOrEmpty(vehicleTranslationName))
                        vehicleTypeName = vehicleTranslationName;

                    item.Id = obj.Id;
                    item.Name = string.Format("{0} - {1}", obj.Code, vehicleTypeName);

                    vehicleTypes.Add(item);
                }
            }
            return Json(vehicleTypes, JsonRequestBehavior.AllowGet);
        }

        #endregion Vehicle Type Load DropDownList
    }
}