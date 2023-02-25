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
    public class ProvinceController : TMSBaseController
    {
        #region Fields

        private readonly IProvinceService _provinceService;
        private readonly IMasterDataTranslationService _masterDataTranslationService;

        #endregion Fields

        #region Constructors

        public ProvinceController(IProvinceService provinceService,
            IMasterDataTranslationService masterDataTranslationService)
        {
            this._provinceService = provinceService;
            this._masterDataTranslationService = masterDataTranslationService;
        }

        #endregion Constructors

        #region Load For DropDownList

        public JsonResult LoadProvinceForDropDownList(int countryId)
        {
            var provinceDropDownList = new List<DropDownListItemExtend>();

            var itemEmpty = new DropDownListItemExtend();
            itemEmpty.Id = 0;
            itemEmpty.Name = "";
            provinceDropDownList.Add(itemEmpty);

            var provinces = _provinceService.GetAllsByCountryId(countryId);

            if (provinces != null && provinces.Count > 0)
            {
                foreach (var obj in provinces)
                {
                    var item = new DropDownListItemExtend();

                    var provinceTranslationName = _masterDataTranslationService.GetName(LanguageCurrent.Id, obj.TranslationId);
                    var provinceName = obj.Name;

                    if (!string.IsNullOrEmpty(provinceTranslationName))
                        provinceName = provinceTranslationName;

                    item.Id = obj.Id;
                    item.Name = provinceName;

                    provinceDropDownList.Add(item);
                }
            }
            return Json(provinceDropDownList, JsonRequestBehavior.AllowGet);
        }

        #endregion Load For DropDownList
    }
}