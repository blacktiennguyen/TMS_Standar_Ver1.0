using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.Core;
using TMS.Core.Domains;
using TMS.Core.Domains.MasterDatas;
using TMS.Core.Extend;
using TMS.Library.Commons;
using TMS.Service.Customers;
using TMS.Service.FixDataTranslations;
using TMS.Service.FIXs;
using TMS.Service.MasterDatas;
using TMS.Service.MasterDataTranslations;
using TMS.Service.Orders;
using TMS.Service.Users;
using TMS.Shared.Const;
using TMS.WebAPP.Framework.Controllers;
using TMS.WebAPP.Models;
using TMS.WebAPP.Models.MasterDataModel;
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

        #region List

        // GET: Route
        public ActionResult List()
        {
            SetActiveFunction(FunctionConst.VehicleDataSetupUrl);
            SetActiveFunctionDataSetup(FunctionConst.VehicleTypeDataSetupUrl);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RouteList(DataSourceRequest command, RouteModel model)
        {
            try
            {
                var items = _routeService.Search(model.Code, model.Name, command.Page - 1, command.PageSize, CompanyCurrent.Id, CompanyCurrent.TenantId);

                var gridModel = new DataSourceResult
                {
                    Data = items.Select(x =>
                    {
                        return new RouteModel
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            NameLL = _masterDataTranslationService.GetName(LanguageCurrent.Id, x.TranslationId),
                            Remark = x.Remark,
                        };
                    }),
                    Total = items.TotalCount
                };

                return Json(gridModel);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));

                return null;
            }
        }

        #endregion List

        #region Save Or Update

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult SaveOrUpdate(RouteModel model)
        {
            try
            {
                //if (!_permissionService.Authorize(StandardPermissionProvider.ManageManufacturers))
                //    return AccessDeniedView();

                var resultId = 0;

                if (model.Id > 0)
                {
                    var updateRoute = _routeService.GetById(model.Id, CompanyCurrent.Id, CompanyCurrent.TenantId);

                    if (updateRoute != null)
                    {
                        updateRoute.Code = model.Code;
                        updateRoute.Name = model.Name;
                        updateRoute.NameLL = model.NameLL;
                        updateRoute.Remark = model.Remark;
                        updateRoute.UpdatedById = UserCurrent.UserId;
                        updateRoute.UpdatedDate = DateTime.Now;

                        resultId = _routeService.SaveOrUpdate(updateRoute);

                        //Save Master Data Translation
                        SaveOrUpdateMasterDataTranslation(_masterDataTranslationService, model.NameLL, updateRoute.TranslationId, "MasterData_Routes");
                    }
                }
                else
                {
                    var routeEntity = new Route();
                    routeEntity.Id = model.Id;
                    routeEntity.Code = model.Code;
                    routeEntity.Name = model.Name;
                    routeEntity.NameLL = model.NameLL;
                    routeEntity.Remark = model.Remark;
                    routeEntity.TranslationId = Guid.NewGuid();
                    routeEntity.CreatedById = UserCurrent.UserId;
                    routeEntity.CreatedDate = DateTime.Now;
                    routeEntity.UpdatedById = null;
                    routeEntity.UpdatedDate = null;
                    routeEntity.CompanyId = CompanyCurrent.Id;
                    routeEntity.TenantId = CompanyCurrent.TenantId;

                    resultId = _routeService.SaveOrUpdate(routeEntity);

                    //Save Master Data Translation
                    SaveOrUpdateMasterDataTranslation(_masterDataTranslationService, model.NameLL, routeEntity.TranslationId, "MasterData_Routes");
                }

                return Json(new { saveSuccess = true, id = resultId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                //ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                if (ex.InnerException.InnerException.Message.Contains("UC_RouteCode"))
                {
                    return Json(new { saveSuccess = false, isDuplicateCode = true, isDuplicate = true }, JsonRequestBehavior.AllowGet);
                }
                else if (ex.InnerException.InnerException.Message.Contains("UC_RouteName"))
                {
                    return Json(new { saveSuccess = false, isDuplicateName = true, isDuplicate = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { saveSuccess = false, isDuplicate = false }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        #endregion Save Or Update

        #region Detail

        //[ValidateAntiForgeryToken]
        public virtual ActionResult Detail(int id)
        {
            try
            {
                //if (!_permissionService.Authorize(StandardPermissionProvider.ManageManufacturers))
                //    return AccessDeniedView();

                var dataResult = new RouteModel();

                var route = _routeService.GetById(id, CompanyCurrent.Id, CompanyCurrent.TenantId);
                if (route == null)
                    return RedirectToAction("List");
                else
                {
                    dataResult.Id = route.Id;
                    dataResult.Code = route.Code;
                    dataResult.Name = route.Name;
                    dataResult.NameLL = _masterDataTranslationService.GetName(LanguageCurrent.Id, route.TranslationId);
                    dataResult.Remark = route.Remark;
                }

                return Json(new { saveSuccess = true, messageId = "MS003", data = dataResult }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(new { saveSuccess = false, messageId = "MS005" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion Detail

        #region Delete, Delete Selected

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(int id)
        {
            try
            {
                //if (!_permissionService.Authorize(StandardPermissionProvider.ManageManufacturers))
                //    return AccessDeniedView();

                var Route = _routeService.GetById(id, CompanyCurrent.Id, CompanyCurrent.TenantId);
                if (Route == null)
                    return RedirectToAction("List");

                _routeService.Delete(Route);

                ////activity log
                //_customerActivityService.InsertActivity("DeleteManufacturer", _localizationService.GetResource("ActivityLog.DeleteManufacturer"), manufacturer.Name);

                DeleteMasterDataTranslation(_masterDataTranslationService, Route.TranslationId);
                return Json(new { saveSuccess = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                //ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                return Json(new { saveSuccess = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            try
            {
                //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                //    return AccessDeniedView();

                if (selectedIds != null)
                {
                    foreach (var id in selectedIds)
                    {
                        var Route = _routeService.GetById(id, CompanyCurrent.Id, CompanyCurrent.TenantId);

                        if (Route != null)
                        {
                            _routeService.Delete(Route);
                            DeleteMasterDataTranslation(_masterDataTranslationService, Route.TranslationId);
                        }
                        else
                            continue;
                    }
                }

                return Json(new { isDeleted = true });
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                //ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                return Json(new { isDeleted = false });
            }
        }

        #endregion Delete, Delete Selected
    }
}