using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains.MasterDatas;

namespace TMS.Service.MasterDatas
{
    public partial interface IDistrictService
    {
        District GetById(int Id);

        List<District> GetAllsByProvinceId(int provinceId);
    }
}