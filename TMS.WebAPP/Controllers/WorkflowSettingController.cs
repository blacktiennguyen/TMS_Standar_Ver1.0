using System.Collections.Generic;
using System.Web.Mvc;
using TMS.Core;
using TMS.Library.Commons;
using TMS.Service.MasterDataTranslations;
using TMS.Service.Workflows;
using TMS.Shared.Const;

namespace TMS.WebAPP.Controllers
{
    public class WorkflowSettingController : TMSBaseController
    {
        #region Fields

        private readonly IWorkflowSettingService _workflowSettingService;
        private readonly IMasterDataTranslationService _masterDataTranslationService;

        #endregion Fields

        #region Constructors

        public WorkflowSettingController(IWorkflowSettingService workflowSettingService,
            IMasterDataTranslationService masterDataTranslationService)
        {
            this._workflowSettingService = workflowSettingService;
            this._masterDataTranslationService = masterDataTranslationService;
        }

        #endregion Constructors

        #region Get List Workflow Setting By FunctionId

        public JsonResult LoadWorkflowSettingByOrderFunctionForDropDownList()
        {
            var workflowSettings = new List<DropDownListItemExtend>();

            var itemEmpty = new DropDownListItemExtend();
            itemEmpty.Id = 0;
            itemEmpty.Name = "";
            workflowSettings.Add(itemEmpty);

            var getWorkflowSettings = _workflowSettingService.GetWorkflowSettingByFunctionId(FunctionConst.Order, CompanyCurrent.Id, CompanyCurrent.TenantId);

            if (getWorkflowSettings != null && getWorkflowSettings.Count > 0)
            {
                foreach (var obj in getWorkflowSettings)
                {
                    var item = new DropDownListItemExtend();

                    var workflowSettingTranslationName = _masterDataTranslationService.GetName(LanguageCurrent.Id, obj.TranslationId);
                    var workflowSettingName = obj.Name;

                    if (!string.IsNullOrEmpty(workflowSettingTranslationName))
                        workflowSettingName = workflowSettingTranslationName;

                    item.Id = obj.Id;
                    item.Name = string.Format("{0} - {1}", obj.Code, workflowSettingName);

                    workflowSettings.Add(item);
                }
            }
            return Json(workflowSettings, JsonRequestBehavior.AllowGet);
        }

        #endregion Get List Workflow Setting By FunctionId
    }
}