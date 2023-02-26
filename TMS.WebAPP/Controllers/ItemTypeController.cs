using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.Core;
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

        // GET: ItemType
        public ActionResult List()
        {
            SetActiveFunction(FunctionConst.OrderDataSetupUrl);
            SetActiveFunctionDataSetup(FunctionConst.ItemTypeOrderDataSetupUrl);

            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
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
    }
}