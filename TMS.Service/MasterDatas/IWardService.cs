using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains.MasterDatas;

namespace TMS.Service.MasterDatas
{
    public partial interface IWardService
    {
        Ward GetById(int Id);

        List<Ward> GetAllsByDistrictId(int districtId);
    }
}