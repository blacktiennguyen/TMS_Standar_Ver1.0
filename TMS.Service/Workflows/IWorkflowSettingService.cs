using System;
using System.Collections.Generic;
using TMS.Core.Domains.Workflows;

namespace TMS.Service.Workflows
{
    public partial interface IWorkflowSettingService
    {
        List<WorkflowSetting> GetWorkflowSettingByFunctionId(int functionId, int companyId, int tenantId);
    }
}