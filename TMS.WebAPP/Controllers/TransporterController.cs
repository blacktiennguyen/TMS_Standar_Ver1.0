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
    public class TransporterController : TMSBaseController
    {
        #region Fields

        private readonly ITransporterService _transporterService;
        private readonly IMasterDataTranslationService _masterDataTranslationService;

        #endregion Fields

        #region Constructors

        public TransporterController(ITransporterService transporterService,
            IMasterDataTranslationService masterDataTranslationService)
        {
            this._transporterService = transporterService;
            this._masterDataTranslationService = masterDataTranslationService;
        }

        #endregion Constructors

        #region Transporter Load DropDownList

        public JsonResult LoadTransporterForDropDownList()
        {
            var transporters = new List<DropDownListItemExtend>();

            var itemEmpty = new DropDownListItemExtend();
            itemEmpty.Id = 0;
            itemEmpty.Name = "";
            transporters.Add(itemEmpty);

            var getTransporters = _transporterService.GetAlls(CompanyCurrent.Id, CompanyCurrent.TenantId);

            if (getTransporters != null && getTransporters.Count > 0)
            {
                foreach (var obj in getTransporters)
                {
                    var item = new DropDownListItemExtend();

                    var transporterTranslationName = _masterDataTranslationService.GetName(LanguageCurrent.Id, obj.TranslationId);
                    var transporterName = obj.Name;

                    if (!string.IsNullOrEmpty(transporterTranslationName))
                        transporterName = transporterTranslationName;

                    item.Id = obj.Id;
                    item.Name = string.Format("{0} - {1}", obj.Code, transporterName);

                    transporters.Add(item);
                }
            }
            return Json(transporters, JsonRequestBehavior.AllowGet);
        }

        #endregion Transporter Load DropDownList
    }
}