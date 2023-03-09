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

        #region List

        // GET: transporter
        public ActionResult List()
        {
            SetActiveFunction(FunctionConst.OrderDataSetupUrl);
            SetActiveFunctionDataSetup(FunctionConst.TransporterOrderDataSetupUrl);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TransporterList(DataSourceRequest command, TransporterModel model)
        {
            try
            {
                var items = _transporterService.Search(model.Code, model.Name, model.Phone, command.Page - 1, command.PageSize, CompanyCurrent.Id, CompanyCurrent.TenantId);

                var gridModel = new DataSourceResult
                {
                    Data = items.Select(x =>
                    {
                        return new TransporterModel
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            NameLL = _masterDataTranslationService.GetName(LanguageCurrent.Id, x.TranslationId),
                            Phone = x.Phone,
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
        public virtual ActionResult SaveOrUpdate(TransporterModel model)
        {
            try
            {
                //if (!_permissionService.Authorize(StandardPermissionProvider.ManageManufacturers))
                //    return AccessDeniedView();

                var resultId = 0;

                if (model.Id > 0)
                {
                    var updateTransporter = _transporterService.GetById(model.Id, CompanyCurrent.Id, CompanyCurrent.TenantId);

                    if (updateTransporter != null)
                    {
                        updateTransporter.Code = model.Code;
                        updateTransporter.Name = model.Name;
                        updateTransporter.NameLL = model.NameLL;
                        updateTransporter.Remark = model.Remark;
                        updateTransporter.Phone = model.Phone;
                        updateTransporter.UpdatedById = UserCurrent.UserId;
                        updateTransporter.UpdatedDate = DateTime.Now;

                        resultId = _transporterService.SaveOrUpdate(updateTransporter);

                        //Save Master Data Translation
                        SaveOrUpdateMasterDataTranslation(_masterDataTranslationService, model.NameLL, updateTransporter.TranslationId, "MasterData_Transporters");
                    }
                }
                else
                {
                    var insertTransporterEntity = new Transporter();
                    insertTransporterEntity.Id = model.Id;
                    insertTransporterEntity.Code = model.Code;
                    insertTransporterEntity.Name = model.Name;
                    insertTransporterEntity.NameLL = model.NameLL;
                    insertTransporterEntity.Remark = model.Remark;
                    insertTransporterEntity.TranslationId = Guid.NewGuid();
                    insertTransporterEntity.Phone = model.Phone;
                    insertTransporterEntity.CreatedById = UserCurrent.UserId;
                    insertTransporterEntity.CreatedDate = DateTime.Now;
                    insertTransporterEntity.UpdatedById = null;
                    insertTransporterEntity.UpdatedDate = null;
                    insertTransporterEntity.CompanyId = CompanyCurrent.Id;
                    insertTransporterEntity.TenantId = CompanyCurrent.TenantId;

                    resultId = _transporterService.SaveOrUpdate(insertTransporterEntity);

                    //Save Master Data Translation
                    SaveOrUpdateMasterDataTranslation(_masterDataTranslationService, model.NameLL, insertTransporterEntity.TranslationId, "MasterData_Transporters");
                }

                return Json(new { saveSuccess = true, id = resultId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                //ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                if (ex.InnerException.InnerException.Message.Contains("UC_TransporterCode"))
                {
                    return Json(new { saveSuccess = false, isDuplicateCode = true, isDuplicate = true }, JsonRequestBehavior.AllowGet);
                }
                else if (ex.InnerException.InnerException.Message.Contains("UC_TransporterName"))
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

                var dataResult = new TransporterModel();

                var transporter = _transporterService.GetById(id, CompanyCurrent.Id, CompanyCurrent.TenantId);
                if (transporter == null)
                    return RedirectToAction("List");
                else
                {
                    dataResult.Id = transporter.Id;
                    dataResult.Code = transporter.Code;
                    dataResult.Name = transporter.Name;
                    dataResult.NameLL = _masterDataTranslationService.GetName(LanguageCurrent.Id, transporter.TranslationId);
                    dataResult.Remark = transporter.Remark;
                    dataResult.Phone = transporter.Phone;
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

                var transporter = _transporterService.GetById(id, CompanyCurrent.Id, CompanyCurrent.TenantId);
                if (transporter == null)
                    return RedirectToAction("List");

                _transporterService.Delete(transporter);

                ////activity log
                //_customerActivityService.InsertActivity("DeleteManufacturer", _localizationService.GetResource("ActivityLog.DeleteManufacturer"), manufacturer.Name);

                DeleteMasterDataTranslation(_masterDataTranslationService, transporter.TranslationId);
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
                        var transporter = _transporterService.GetById(id, CompanyCurrent.Id, CompanyCurrent.TenantId);

                        if (transporter != null)
                        {
                            _transporterService.Delete(transporter);
                            DeleteMasterDataTranslation(_masterDataTranslationService, transporter.TranslationId);
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