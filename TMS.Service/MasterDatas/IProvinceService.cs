using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains.MasterDatas;

namespace TMS.Service.MasterDatas
{
    public partial interface IProvinceService
    {
        Province GetById(int Id);

        List<Province> GetAllsByCountryId(int countryId);
    }
}