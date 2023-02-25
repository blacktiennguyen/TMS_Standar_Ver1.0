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
    public class DistrictController : TMSBaseController
    {
        #region Fields

        private readonly IDistrictService _districtService;
        private readonly IMasterDataTranslationService _masterDataTranslationService;

        #endregion Fields

        #region Constructors

        public DistrictController(IDistrictService districtService,
            IMasterDataTranslationService masterDataTranslationService)
        {
            this._districtService = districtService;
            this._masterDataTranslationService = masterDataTranslationService;
        }

        #endregion Constructors

        #region Load For DropDownList

        public JsonResult LoadDistrictForDropDownList(int provinceId)
        {
            try
            {
                var districtDropDownList = new List<DropDownListItemExtend>();

                var itemEmpty = new DropDownListItemExtend();
                itemEmpty.Id = 0;
                itemEmpty.Name = "";
                districtDropDownList.Add(itemEmpty);

                var districts = _districtService.GetAllsByProvinceId(provinceId);

                if (districts != null && districts.Count > 0)
                {
                    foreach (var obj in districts)
                    {
                        var item = new DropDownListItemExtend();

                        var districtTranslationName = _masterDataTranslationService.GetName(LanguageCurrent.Id, obj.TranslationId);
                        var districtName = obj.Name;

                        if (!string.IsNullOrEmpty(districtTranslationName))
                            districtName = districtTranslationName;

                        item.Id = obj.Id;
                        item.Name = districtName;

                        districtDropDownList.Add(item);
                    }
                }
                return Json(districtDropDownList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion Load For DropDownList
    }
}