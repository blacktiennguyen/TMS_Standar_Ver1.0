using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.Core;
using TMS.Core.Domains.MasterDatas;
using TMS.Service.MasterDatas;
using TMS.Service.MasterDataTranslations;
using TMS.Shared.Const;
using TMS.WebAPP.Models.MasterDataModel;

namespace TMS.WebAPP.Controllers
{
    public class ItemUnitController : TMSBaseController
    {
        // GET: ItemUnit

        #region Fields

        private readonly IItemUnitService _itemUnitService;
        private readonly IMasterDataTranslationService _masterDataTranslationService;

        #endregion Fields

        #region Constructors

        public ItemUnitController(IItemUnitService itemUnitService, IMasterDataTranslationService masterDataTranslationService)
        {
            this._itemUnitService = itemUnitService;
            this._masterDataTranslationService = masterDataTranslationService;
        }

        #endregion Constructors

        #region List

        // GET: ItemUnit
        public ActionResult List()
        {
            SetActiveFunction(FunctionConst.OrderDataSetupUrl);
            SetActiveFunctionDataSetup(FunctionConst.ItemUnitOrderDataSetupUrl);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ItemUnitList(DataSourceRequest command, ItemUnitModel model)
        {
            try
            {
                var items = _itemUnitService.Search(model.Code, model.Name, command.Page - 1, command.PageSize, CompanyCurrent.Id, CompanyCurrent.TenantId);

                var gridModel = new DataSourceResult
                {
                    Data = items.Select(x =>
                    {
                        return new ItemUnitModel
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
        public virtual ActionResult SaveOrUpdate(ItemUnitModel model)
        {
            try
            {
                //if (!_permissionService.Authorize(StandardPermissionProvider.ManageManufacturers))
                //    return AccessDeniedView();

                var resultId = 0;

                if (model.Id > 0)
                {
                    var updateItemUnit = _itemUnitService.GetById(model.Id, CompanyCurrent.Id, CompanyCurrent.TenantId);

                    if (updateItemUnit != null)
                    {
                        updateItemUnit.Code = model.Code;
                        updateItemUnit.Name = model.Name;
                        updateItemUnit.NameLL = model.NameLL;
                        updateItemUnit.Remark = model.Remark;
                        updateItemUnit.UpdatedById = UserCurrent.UserId;
                        updateItemUnit.UpdatedDate = DateTime.Now;

                        resultId = _itemUnitService.SaveOrUpdate(updateItemUnit);

                        //Save Master Data Translation
                        SaveOrUpdateMasterDataTranslation(_masterDataTranslationService, model.NameLL, updateItemUnit.TranslationId, "MasterData_ItemUnits");
                    }
                }
                else
                {
                    var itemUnitEntity = new ItemUnit();
                    itemUnitEntity.Id = model.Id;
                    itemUnitEntity.Code = model.Code;
                    itemUnitEntity.Name = model.Name;
                    itemUnitEntity.NameLL = model.NameLL;
                    itemUnitEntity.Remark = model.Remark;
                    itemUnitEntity.TranslationId = Guid.NewGuid();
                    itemUnitEntity.CreatedById = UserCurrent.UserId;
                    itemUnitEntity.CreatedDate = DateTime.Now;
                    itemUnitEntity.UpdatedById = null;
                    itemUnitEntity.UpdatedDate = null;
                    itemUnitEntity.CompanyId = CompanyCurrent.Id;
                    itemUnitEntity.TenantId = CompanyCurrent.TenantId;

                    resultId = _itemUnitService.SaveOrUpdate(itemUnitEntity);

                    //Save Master Data Translation
                    SaveOrUpdateMasterDataTranslation(_masterDataTranslationService, model.NameLL, itemUnitEntity.TranslationId, "MasterData_ItemUnits");
                }

                return Json(new { saveSuccess = true, id = resultId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                //ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                if (ex.InnerException.InnerException.Message.Contains("UC_ItemUnitCode"))
                {
                    return Json(new { saveSuccess = false, isDuplicateCode = true, isDuplicate = true }, JsonRequestBehavior.AllowGet);
                }
                else if (ex.InnerException.InnerException.Message.Contains("UC_ItemUnitName"))
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

                var dataResult = new ItemUnitModel();

                var itemUnit = _itemUnitService.GetById(id, CompanyCurrent.Id, CompanyCurrent.TenantId);
                if (itemUnit == null)
                    return RedirectToAction("List");
                else
                {
                    dataResult.Id = itemUnit.Id;
                    dataResult.Code = itemUnit.Code;
                    dataResult.Name = itemUnit.Name;
                    dataResult.NameLL = _masterDataTranslationService.GetName(LanguageCurrent.Id, itemUnit.TranslationId);
                    dataResult.Remark = itemUnit.Remark;
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

                var itemUnit = _itemUnitService.GetById(id, CompanyCurrent.Id, CompanyCurrent.TenantId);
                if (itemUnit == null)
                    return RedirectToAction("List");

                _itemUnitService.Delete(itemUnit);

                ////activity log
                //_customerActivityService.InsertActivity("DeleteManufacturer", _localizationService.GetResource("ActivityLog.DeleteManufacturer"), manufacturer.Name);

                DeleteMasterDataTranslation(_masterDataTranslationService, itemUnit.TranslationId);
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
                        var itemUnit = _itemUnitService.GetById(id, CompanyCurrent.Id, CompanyCurrent.TenantId);

                        if (itemUnit != null)
                        {
                            _itemUnitService.Delete(itemUnit);
                            DeleteMasterDataTranslation(_masterDataTranslationService, itemUnit.TranslationId);
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