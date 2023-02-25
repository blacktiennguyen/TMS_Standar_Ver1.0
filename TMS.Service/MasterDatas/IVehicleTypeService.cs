using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains.MasterDatas;

namespace TMS.Service.MasterDatas
{
    public partial interface IVehicleTypeService
    {
        VehicleType GetById(int Id);

        List<VehicleType> GetAlls(int companyId, int tenantId);
    }
}