using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Core.Domains;

namespace TMS.Service.Customers
{
    public partial interface ICustomerService
    {
        Customer GetById(int Id, int companyId, int tenantId);

        IPagedList<Customer> SearchChooseCustomer(int pageIndex = 0, int pageSize = int.MaxValue, int companyId = 0, int tenantId = 0);
    }
}