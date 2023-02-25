using System;
using TMS.Core.Domains.FIXs;

namespace TMS.Service.FIXs
{
    public partial interface IOrderShippingStatusService
    {
        OrderShippingStatus GetById(int Id);
    }
}