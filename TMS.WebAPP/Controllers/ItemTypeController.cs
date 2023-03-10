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
    public class ItemTypeController : TMSBaseController
    {
        #region Fields

        private readonly IItemTypeService _itemTypeService;
        private readonly IMasterDataTranslationService _masterDataTranslationService;

        #endregion Fields

        #region Constructors

        public ItemTypeController(IItemTypeService itemTypeService, IMasterDataTranslationService masterDataTranslationService)
        {
            this._itemTypeService = itemTypeService;
            this._masterDataTranslationService = masterDataTranslationService;
        }

        #endregion Constructors

        #region List

        // GET: ItemType
        public ActionResult List()
        {
            SetActiveFunction(FunctionConst.OrderDataSetupUrl);
            SetActiveFunctionDataSetup(FunctionConst.ItemTypeOrderDataSetupUrl);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ItemTypeList(DataSourceRequest command, ItemTypeModel model)
        {
            try
            {
                var items = _itemTypeService.Search(model.Code, model.Name, command.Page - 1, command.PageSize, CompanyCurrent.Id, CompanyCurrent.TenantId);

                var gridModel = new DataSourceResult
                {
                    Data = items.Select(x =>
                    {
                        return new ItemTypeModel
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
        public virtual ActionResult SaveOrUpdate(ItemTypeModel model)
        {
            try
            {
                //if (!_permissionService.Authorize(StandardPermissionProvider.ManageManufacturers))
                //    return AccessDeniedView();

                var resultId = 0;

                if (model.Id > 0)
                {
                    var updateItemType = _itemTypeService.GetById(model.Id, CompanyCurrent.Id, CompanyCurrent.TenantId);

                    if (updateItemType != null)
                    {
                        updateItemType.Code = model.Code;
                        updateItemType.Name = model.Name;
                        updateItemType.NameLL = model.NameLL;
                        updateItemType.Remark = model.Remark;
                        updateItemType.UpdatedById = UserCurrent.UserId;
                        updateItemType.UpdatedDate = DateTime.Now;

                        resultId = _itemTypeService.SaveOrUpdate(updateItemType);

                        //Save Master Data Translation
                        SaveOrUpdateMasterDataTranslation(_masterDataTranslationService, model.NameLL, updateItemType.TranslationId, "MasterData_ItemTypes");
                    }
                }
                else
                {
                    var itemTypeEntity = new ItemType();
                    itemTypeEntity.Id = model.Id;
                    itemTypeEntity.Code = model.Code;
                    itemTypeEntity.Name = model.Name;
                    itemTypeEntity.NameLL = model.NameLL;
                    itemTypeEntity.Remark = model.Remark;
                    itemTypeEntity.TranslationId = Guid.NewGuid();
                    itemTypeEntity.CreatedById = UserCurrent.UserId;
                    itemTypeEntity.CreatedDate = DateTime.Now;
                    itemTypeEntity.UpdatedById = null;
                    itemTypeEntity.UpdatedDate = null;
                    itemTypeEntity.CompanyId = CompanyCurrent.Id;
                    itemTypeEntity.TenantId = CompanyCurrent.TenantId;

                    resultId = _itemTypeService.SaveOrUpdate(itemTypeEntity);

                    //Save Master Data Translation
                    SaveOrUpdateMasterDataTranslation(_masterDataTranslationService, model.NameLL, itemTypeEntity.TranslationId, "MasterData_ItemTypes");
                }

                return Json(new { saveSuccess = true, id = resultId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                //ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                if (ex.InnerException.InnerException.Message.Contains("UC_ItemTypeCode"))
                {
                    return Json(new { saveSuccess = false, isDuplicateCode = true, isDuplicate = true }, JsonRequestBehavior.AllowGet);
                }
                else if (ex.InnerException.InnerException.Message.Contains("UC_ItemTypeName"))
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

                var dataResult = new ItemTypeModel();

                var itemType = _itemTypeService.GetById(id, CompanyCurrent.Id, CompanyCurrent.TenantId);
                if (itemType == null)
                    return RedirectToAction("List");
                else
                {
                    dataResult.Id = itemType.Id;
                    dataResult.Code = itemType.Code;
                    dataResult.Name = itemType.Name;
                    dataResult.NameLL = _masterDataTranslationService.GetName(LanguageCurrent.Id, itemType.TranslationId);
                    dataResult.Remark = itemType.Remark;
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

                var itemType = _itemTypeService.GetById(id, CompanyCurrent.Id, CompanyCurrent.TenantId);
                if (itemType == null)
                    return RedirectToAction("List");

                _itemTypeService.Delete(itemType);

                ////activity log
                //_customerActivityService.InsertActivity("DeleteManufacturer", _localizationService.GetResource("ActivityLog.DeleteManufacturer"), manufacturer.Name);

                DeleteMasterDataTranslation(_masterDataTranslationService, itemType.TranslationId);
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
                        var itemType = _itemTypeService.GetById(id, CompanyCurrent.Id, CompanyCurrent.TenantId);

                        if (itemType != null)
                        {
                            _itemTypeService.Delete(itemType);
                            DeleteMasterDataTranslation(_masterDataTranslationService, itemType.TranslationId);
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