using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Core.Domains.MasterDatas;

namespace TMS.Service.MasterDatas
{
    public partial interface IRouteService
    {
        List<Route> GetAlls(int companyId, int tenantId);

        Route GetById(int id, int companyId = 0, int tenantId = 0);

        IPagedList<Route> Search(string code, string name, int pageIndex = 0, int pageSize = int.MaxValue, int companyId = 0, int tenantId = 0);

        int SaveOrUpdate(Route itemType);

        void Delete(Route itemType);
    }
}