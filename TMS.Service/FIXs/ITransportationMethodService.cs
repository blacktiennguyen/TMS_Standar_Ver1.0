using System;
using System.Collections.Generic;
using TMS.Core.Domains.FIXs;

namespace TMS.Service.FIXs
{
    public partial interface ITransportationMethodService
    {
        List<TransportationMethod> GetAlls();
    }
}