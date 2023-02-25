using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Core.Domains;
using TMS.Core.Extend;

namespace TMS.Service.Orders
{
    public partial interface IOrderService
    {
        Order GetById(int Id, int companyId, int tenantId);

        Order GetByOrderCode(string orderCode, int companyId, int tenantId);

        bool CheckExistData(int id = 0, string name = "", int companyId = 0, int tenantId = 0);

        IPagedList<Order> SearchOrder(OrderSearchExtend orderSearchExtend, int pageIndex = 0, int pageSize = int.MaxValue, int companyId = 0, int tenantId = 0);

        void InsertOrder(Order order);

        void UpdateOrder(Order order);

        void DeleteOrder(Order order);
    }
}