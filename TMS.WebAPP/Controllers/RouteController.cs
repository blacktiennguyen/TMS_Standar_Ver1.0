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
    public class RouteController : TMSBaseController
    {
        #region Fields

        private readonly IRouteService _routeService;
        private readonly IMasterDataTranslationService _masterDataTranslationService;

        #endregion Fields

        #region Constructors

        public RouteController(IRouteService routeService,
            IMasterDataTranslationService masterDataTranslationService)
        {
            this._routeService = routeService;
            this._masterDataTranslationService = masterDataTranslationService;
        }

        #endregion Constructors

        #region Route Load DropDownList

        public JsonResult LoadRouteForDropDownList()
        {
            var routes = new List<DropDownListItemExtend>();

            var itemEmpty = new DropDownListItemExtend();
            itemEmpty.Id = 0;
            itemEmpty.Name = "";
            routes.Add(itemEmpty);

            var getRoutes = _routeService.GetAlls(CompanyCurrent.Id, CompanyCurrent.TenantId);

            if (getRoutes != null && getRoutes.Count > 0)
            {
                foreach (var obj in getRoutes)
                {
                    var item = new DropDownListItemExtend();

                    var routeTranslationName = _masterDataTranslationService.GetName(LanguageCurrent.Id, obj.TranslationId);
                    var routeName = obj.Name;

                    if (!string.IsNullOrEmpty(routeTranslationName))
                        routeName = routeTranslationName;

                    item.Id = obj.Id;
                    item.Name = string.Format("{0} - {1}", obj.Code, routeName);

                    routes.Add(item);
                }
            }
            return Json(routes, JsonRequestBehavior.AllowGet);
        }

        #endregion Route Load DropDownList
    }
}