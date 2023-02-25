using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Core.Domains.Workflows;

namespace TMS.Service.Workflows
{
    public partial class WorkflowSettingService : IWorkflowSettingService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<WorkflowSetting> _workflowSettingRepository;

        #endregion Fields

        #region Ctor

        public WorkflowSettingService(IRepository<WorkflowSetting> workflowSettingRepository)
        {
            this._workflowSettingRepository = workflowSettingRepository;
        }

        #endregion Ctor

        public List<WorkflowSetting> GetWorkflowSettingByFunctionId(int functionId, int companyId, int tenantId)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var workflowSettings = db.WorkflowSettings
                        .Where(x => x.FunctionId == functionId &&
                                    x.IsActive == true &&
                                    x.CompanyId == companyId && x.TenantId == tenantId)
                        .ToList();

                    return workflowSettings;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }
    }
}