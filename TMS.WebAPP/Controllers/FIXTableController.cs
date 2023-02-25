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
    public class FIXTableController : TMSBaseController
    {
        #region Fields

        private readonly IWeightTypeService _weightTypeService;
        private readonly IPayerPostageServiceService _payerPostageServiceService;
        private readonly IFixDataTranslationService _fixDataTranslationService;
        private readonly TransportationMethodService _transportationMethodService;

        #endregion Fields

        #region Constructors

        public FIXTableController(IWeightTypeService weightTypeService,
            IPayerPostageServiceService payerPostageServiceService,
            IFixDataTranslationService fixDataTranslationService,
            TransportationMethodService transportationMethodService)
        {
            this._weightTypeService = weightTypeService;
            this._payerPostageServiceService = payerPostageServiceService;
            this._fixDataTranslationService = fixDataTranslationService;
            this._transportationMethodService = transportationMethodService;
        }

        #endregion Constructors

        #region Weight Type Load DropDownList

        public JsonResult LoadWeightTypeForDropDownList()
        {
            var weightTypes = new List<DropDownListItemExtend>();

            var getWeightTypes = _weightTypeService.GetAlls();

            if (getWeightTypes != null && getWeightTypes.Count > 0)
            {
                foreach (var obj in getWeightTypes)
                {
                    var item = new DropDownListItemExtend();

                    var weightTranslationName = _fixDataTranslationService.GetName(LanguageCurrent.Id, obj.TranslationId);
                    var weightTypeName = obj.Name;

                    if (!string.IsNullOrEmpty(weightTranslationName))
                        weightTypeName = weightTranslationName;

                    item.Id = obj.Id;
                    item.Name = string.Format("{0}", weightTypeName);

                    weightTypes.Add(item);
                }
            }
            return Json(weightTypes, JsonRequestBehavior.AllowGet);
        }

        #endregion Weight Type Load DropDownList

        #region PayerPostageService Load DropDownList

        public JsonResult LoadPayerPostageServiceForDropDownList()
        {
            var payerPostageServices = new List<DropDownListItemExtend>();

            var getPayerPostageServices = _payerPostageServiceService.GetAlls();

            if (getPayerPostageServices != null && getPayerPostageServices.Count > 0)
            {
                foreach (var obj in getPayerPostageServices)
                {
                    var item = new DropDownListItemExtend();

                    var payerTranslationName = _fixDataTranslationService.GetName(LanguageCurrent.Id, obj.TranslationId);
                    var payerPostageServiceName = obj.Name;

                    if (!string.IsNullOrEmpty(payerTranslationName))
                        payerPostageServiceName = payerTranslationName;

                    item.Id = obj.Id;
                    item.Name = string.Format("{0} - {1}", obj.Code, payerPostageServiceName);

                    payerPostageServices.Add(item);
                }
            }
            return Json(payerPostageServices, JsonRequestBehavior.AllowGet);
        }

        #endregion PayerPostageService Load DropDownList

        #region TransportationMethod Load DropDownList

        public JsonResult LoadTransportationMethodForDropDownList()
        {
            var transportationMethods = new List<DropDownListItemExtend>();

            var getTransportationMethods = _transportationMethodService.GetAlls();

            if (getTransportationMethods != null && getTransportationMethods.Count > 0)
            {
                foreach (var obj in getTransportationMethods)
                {
                    var item = new DropDownListItemExtend();

                    var transportationMethodTranslationName = _fixDataTranslationService.GetName(LanguageCurrent.Id, obj.TranslationId);
                    var transportationMethodeName = obj.Name;

                    if (!string.IsNullOrEmpty(transportationMethodTranslationName))
                        transportationMethodeName = transportationMethodTranslationName;

                    item.Id = obj.Id;
                    item.Name = string.Format("{0} - {1}", obj.Code, transportationMethodeName);

                    transportationMethods.Add(item);
                }
            }
            return Json(transportationMethods, JsonRequestBehavior.AllowGet);
        }

        #endregion TransportationMethod Load DropDownList
    }
}