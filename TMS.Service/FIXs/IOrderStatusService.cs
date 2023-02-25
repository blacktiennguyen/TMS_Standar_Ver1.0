using System;
using TMS.Core.Domains.FIXs;

namespace TMS.Service.FIXs
{
    public partial interface IOrderStatusService
    {
        OrderStatus GetById(int Id);
    }
}