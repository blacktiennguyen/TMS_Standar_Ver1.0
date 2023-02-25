using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Core.Domains;

namespace TMS.Service.SystemSettings
{
    public partial class SystemSettingService : ISystemSettingService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<SystemSetting> _systemSettingRepository;

        #endregion Fields

        #region Ctor

        public SystemSettingService(IRepository<SystemSetting> systemSettingRepository)
        {
            this._systemSettingRepository = systemSettingRepository;
        }

        #endregion Ctor

        public SystemSetting GetByKey(string key, int companyId, int tenantId)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var systemSetting = db.SystemSettings
                        .Where(x => x.Key == key && x.CompanyId == companyId && x.TenantId == tenantId)
                        .FirstOrDefault();

                    return systemSetting;
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