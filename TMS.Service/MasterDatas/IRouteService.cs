using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains.MasterDatas;

namespace TMS.Service.MasterDatas
{
    public partial interface IRouteService
    {
        Route GetById(int Id);

        List<Route> GetAlls(int companyId, int tenantId);
    }
}