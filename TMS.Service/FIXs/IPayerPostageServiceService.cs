using System;
using System.Collections.Generic;
using TMS.Core.Domains.FIXs;

namespace TMS.Service.FIXs
{
    public partial interface IPayerPostageServiceService
    {
        List<PayerPostageService> GetAlls();
    }
}