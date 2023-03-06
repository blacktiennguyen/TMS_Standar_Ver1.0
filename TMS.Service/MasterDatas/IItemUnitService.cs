using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Core.Domains.MasterDatas;

namespace TMS.Service.MasterDatas
{
    public partial interface IItemUnitService
    {
        ItemUnit GetById(int id, int companyId = 0, int tenantId = 0);

        IPagedList<ItemUnit> Search(string code, string name, int pageIndex = 0, int pageSize = int.MaxValue, int companyId = 0, int tenantId = 0);

        int SaveOrUpdate(ItemUnit itemType);

        void Delete(ItemUnit itemType);
    }
}