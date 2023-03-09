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

        #region List

        // GET: VehicleType
        public ActionResult List()
        {
            SetActiveFunction(FunctionConst.VehicleDataSetupUrl);
            SetActiveFunctionDataSetup(FunctionConst.VehicleTypeDataSetupUrl);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VehicleTypeList(DataSourceRequest command, VehicleTypeModel model)
        {
            try
            {
                var items = _vehicleTypeService.Search(model.Code, model.Name, command.Page - 1, command.PageSize, CompanyCurrent.Id, CompanyCurrent.TenantId);

                var gridModel = new DataSourceResult
                {
                    Data = items.Select(x =>
                    {
                        return new VehicleTypeModel
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
        public virtual ActionResult SaveOrUpdate(VehicleTypeModel model)
        {
            try
            {
                //if (!_permissionService.Authorize(StandardPermissionProvider.ManageManufacturers))
                //    return AccessDeniedView();

                var resultId = 0;

                if (model.Id > 0)
                {
                    var updateVehicleType = _vehicleTypeService.GetById(model.Id, CompanyCurrent.Id, CompanyCurrent.TenantId);

                    if (updateVehicleType != null)
                    {
                        updateVehicleType.Code = model.Code;
                        updateVehicleType.Name = model.Name;
                        updateVehicleType.NameLL = model.NameLL;
                        updateVehicleType.Remark = model.Remark;
                        updateVehicleType.UpdatedById = UserCurrent.UserId;
                        updateVehicleType.UpdatedDate = DateTime.Now;

                        resultId = _vehicleTypeService.SaveOrUpdate(updateVehicleType);

                        //Save Master Data Translation
                        SaveOrUpdateMasterDataTranslation(_masterDataTranslationService, model.NameLL, updateVehicleType.TranslationId, "MasterData_VehicleTypes");
                    }
                }
                else
                {
                    var vehicleTypeEntity = new VehicleType();
                    vehicleTypeEntity.Id = model.Id;
                    vehicleTypeEntity.Code = model.Code;
                    vehicleTypeEntity.Name = model.Name;
                    vehicleTypeEntity.NameLL = model.NameLL;
                    vehicleTypeEntity.Remark = model.Remark;
                    vehicleTypeEntity.TranslationId = Guid.NewGuid();
                    vehicleTypeEntity.CreatedById = UserCurrent.UserId;
                    vehicleTypeEntity.CreatedDate = DateTime.Now;
                    vehicleTypeEntity.UpdatedById = null;
                    vehicleTypeEntity.UpdatedDate = null;
                    vehicleTypeEntity.CompanyId = CompanyCurrent.Id;
                    vehicleTypeEntity.TenantId = CompanyCurrent.TenantId;

                    resultId = _vehicleTypeService.SaveOrUpdate(vehicleTypeEntity);

                    //Save Master Data Translation
                    SaveOrUpdateMasterDataTranslation(_masterDataTranslationService, model.NameLL, vehicleTypeEntity.TranslationId, "MasterData_VehicleTypes");
                }

                return Json(new { saveSuccess = true, id = resultId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                //ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                if (ex.InnerException.InnerException.Message.Contains("UC_VehicleTypeCode"))
                {
                    return Json(new { saveSuccess = false, isDuplicateCode = true, isDuplicate = true }, JsonRequestBehavior.AllowGet);
                }
                else if (ex.InnerException.InnerException.Message.Contains("UC_VehicleTypeName"))
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

                var dataResult = new VehicleTypeModel();

                var vehicleType = _vehicleTypeService.GetById(id, CompanyCurrent.Id, CompanyCurrent.TenantId);
                if (vehicleType == null)
                    return RedirectToAction("List");
                else
                {
                    dataResult.Id = vehicleType.Id;
                    dataResult.Code = vehicleType.Code;
                    dataResult.Name = vehicleType.Name;
                    dataResult.NameLL = _masterDataTranslationService.GetName(LanguageCurrent.Id, vehicleType.TranslationId);
                    dataResult.Remark = vehicleType.Remark;
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

                var vehicleType = _vehicleTypeService.GetById(id, CompanyCurrent.Id, CompanyCurrent.TenantId);
                if (vehicleType == null)
                    return RedirectToAction("List");

                _vehicleTypeService.Delete(vehicleType);

                ////activity log
                //_customerActivityService.InsertActivity("DeleteManufacturer", _localizationService.GetResource("ActivityLog.DeleteManufacturer"), manufacturer.Name);

                DeleteMasterDataTranslation(_masterDataTranslationService, vehicleType.TranslationId);
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
                        var vehicleType = _vehicleTypeService.GetById(id, CompanyCurrent.Id, CompanyCurrent.TenantId);

                        if (vehicleType != null)
                        {
                            _vehicleTypeService.Delete(vehicleType);
                            DeleteMasterDataTranslation(_masterDataTranslationService, vehicleType.TranslationId);
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