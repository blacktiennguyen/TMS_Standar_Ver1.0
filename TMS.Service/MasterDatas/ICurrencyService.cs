using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains.MasterDatas;

namespace TMS.Service.MasterDatas
{
    public partial interface ICurrencyService
    {
        Currency GetLocalCurrencyByCompanyId(int companyId, int tenantId);

        Currency GetById(int Id, int companyId, int tenantId);

        List<Currency> GetAlls(int companyId, int tenantId);
    }
}