using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains;

namespace TMS.Service.SystemSettings
{
    public partial interface ISystemSettingService
    {
        SystemSetting GetByKey(string key, int companyId, int tenantId);
    }
}