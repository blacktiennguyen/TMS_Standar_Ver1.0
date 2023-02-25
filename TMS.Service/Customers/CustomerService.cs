using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Core.Domains;

namespace TMS.Service.Customers
{
    public class CustomerService : ICustomerService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<Customer> _customerRepository;

        #endregion Fields

        #region Ctor

        public CustomerService(IRepository<Customer> customerRepository)
        {
            this._customerRepository = customerRepository;
        }

        #endregion Ctor

        public Customer GetById(int Id, int companyId, int tenantId)
        {
            var customer = new Customer();
            try
            {
                using (var db = new TMSContext())
                {
                    customer = db.Customers
                        .Where(x => x.Id == Id && x.CompanyId == companyId && x.TenantId == tenantId)
                        .FirstOrDefault();

                    return customer;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public IPagedList<Customer> SearchChooseCustomer(int pageIndex = 0, int pageSize = int.MaxValue, int companyId = 0, int tenantId = 0)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var query = db.Customers
                        .Where(x => x.CompanyId == companyId && x.TenantId == tenantId)
                        .ToList();

                    //if (!String.IsNullOrEmpty(orderCode))
                    //    query = query.Where(x => x.OrderCode != null && !String.IsNullOrEmpty(x.OrderCode) && x.OrderCode.Contains(orderCode))
                    //            .ToList();

                    query = query.OrderByDescending(x => x.CreatedDate).ToList();

                    return new PagedList<Customer>(query, pageIndex, pageSize);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }
    }
}