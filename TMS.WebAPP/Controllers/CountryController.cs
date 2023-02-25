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
    public class CountryController : TMSBaseController
    {
        #region Fields

        private readonly ICountryService _countryService;
        private readonly IMasterDataTranslationService _masterDataTranslationService;

        #endregion Fields

        #region Constructors

        public CountryController(ICountryService countryService,
            IMasterDataTranslationService masterDataTranslationService)
        {
            this._countryService = countryService;
            this._masterDataTranslationService = masterDataTranslationService;
        }

        #endregion Constructors

        #region Load For DropDownList

        public JsonResult LoadCountryForDropDownList()
        {
            var countryDropDownList = new List<DropDownListItemExtend>();

            var itemEmpty = new DropDownListItemExtend();
            itemEmpty.Id = 0;
            itemEmpty.Name = "";
            countryDropDownList.Add(itemEmpty);

            var countries = _countryService.GetAlls();

            if (countries != null && countries.Count > 0)
            {
                foreach (var obj in countries)
                {
                    var item = new DropDownListItemExtend();

                    var countryTranslationName = _masterDataTranslationService.GetName(LanguageCurrent.Id, obj.TranslationId);
                    var countryName = obj.Name;

                    if (!string.IsNullOrEmpty(countryTranslationName))
                        countryName = countryTranslationName;

                    item.Id = obj.Id;
                    item.Name = countryName;

                    countryDropDownList.Add(item);
                }
            }
            return Json(countryDropDownList, JsonRequestBehavior.AllowGet);
        }

        #endregion Load For DropDownList
    }
}