using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Core.Domains.MasterDatas;

namespace TMS.Service.MasterDatas
{
    public partial interface ITransporterService
    {
        List<Transporter> GetAlls(int companyId, int tenantId);

        Transporter GetById(int id, int companyId = 0, int tenantId = 0);

        IPagedList<Transporter> Search(string code, string name, string phone, int pageIndex = 0, int pageSize = int.MaxValue, int companyId = 0, int tenantId = 0);

        int SaveOrUpdate(Transporter transporter);

        void Delete(Transporter transporter);
    }
}