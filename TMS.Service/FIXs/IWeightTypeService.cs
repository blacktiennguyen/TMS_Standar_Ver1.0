using System;
using System.Collections.Generic;
using TMS.Core.Domains.FIXs;

namespace TMS.Service.FIXs
{
    public partial interface IWeightTypeService
    {
        WeightType GetById(int Id);

        List<WeightType> GetAlls();
    }
}