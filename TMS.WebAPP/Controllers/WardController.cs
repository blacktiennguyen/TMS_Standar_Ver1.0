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
    public class WardController : TMSBaseController
    {
        #region Fields

        private readonly IWardService _wardService;
        private readonly IMasterDataTranslationService _masterDataTranslationService;

        #endregion Fields

        #region Constructors

        public WardController(IWardService wardService,
            IMasterDataTranslationService masterDataTranslationService)
        {
            this._wardService = wardService;
            this._masterDataTranslationService = masterDataTranslationService;
        }

        #endregion Constructors

        #region Load For DropDownList

        public JsonResult LoadWardForDropDownList(int districtId)
        {
            var wardDropDownList = new List<DropDownListItemExtend>();

            var itemEmpty = new DropDownListItemExtend();
            itemEmpty.Id = 0;
            itemEmpty.Name = "";
            wardDropDownList.Add(itemEmpty);

            var wards = _wardService.GetAllsByDistrictId(districtId);

            if (wards != null && wards.Count > 0)
            {
                foreach (var obj in wards)
                {
                    var item = new DropDownListItemExtend();

                    var wardTranslationName = _masterDataTranslationService.GetName(LanguageCurrent.Id, obj.TranslationId);
                    var wardName = obj.Name;

                    if (!string.IsNullOrEmpty(wardTranslationName))
                        wardName = wardTranslationName;

                    item.Id = obj.Id;
                    item.Name = wardName;

                    wardDropDownList.Add(item);
                }
            }
            return Json(wardDropDownList, JsonRequestBehavior.AllowGet);
        }

        #endregion Load For DropDownList
    }
}