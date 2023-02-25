using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Core.Domains;

namespace TMS.Service.Orders
{
    public class OrderItemDetailService : IOrderItemDetailService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<OrderItemDetail> _orderItemDetailRepository;

        #endregion Fields

        #region Ctor

        public OrderItemDetailService(IRepository<OrderItemDetail> orderItemDetailRepository)
        {
            _orderItemDetailRepository = orderItemDetailRepository;
        }

        #endregion Ctor

        public List<OrderItemDetail> GetByOrderItemId(int orderId, int companyId, int tenantId)
        {
            var orderItemDetail = new List<OrderItemDetail>();
            try
            {
                using (var db = new TMSContext())
                {
                    orderItemDetail = db.OrderItemDetails
                        .Where(x => x.OrderId == orderId && x.CompanyId == companyId && x.TenantId == tenantId)
                        .ToList();

                    return orderItemDetail;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        //public bool CheckExistData(int id = 0, string code = "", int companyId = 0, int tenantId = 0)
        //{
        //    var isExist = false;
        //    try
        //    {
        //        using (var db = new TMSContext())
        //        {
        //            isExist = db.Orders
        //                .Where(x => x.Id != id && x.OrderCode == code && x.CompanyId == companyId && x.TenantId == tenantId)
        //                .Any();

        //            return isExist;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message);
        //        return false;
        //    }
        //}

        //public IPagedList<Order> SearchOrder(string orderCode, int pageIndex = 0, int pageSize = int.MaxValue, int companyId = 0, int tenantId = 0)
        //{
        //    try
        //    {
        //        using (var db = new TMSContext())
        //        {
        //            var query = db.Orders
        //                .Where(x => x.CompanyId == companyId && x.TenantId == tenantId)
        //                .ToList();

        //            if (!String.IsNullOrEmpty(orderCode))
        //                query = query.Where(x => x.OrderCode != null && !String.IsNullOrEmpty(x.OrderCode) && x.OrderCode.Contains(orderCode))
        //                        .ToList();

        //            query = query.OrderByDescending(x => x.CreatedDate).ToList();

        //            return new PagedList<Order>(query, pageIndex, pageSize);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message);
        //        throw;
        //    }
        //}

        //#region Insert / Update / Delete

        //public void InsertOrder(Order order)
        //{
        //    try
        //    {
        //        _orderRepository.Insert(order);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message);
        //        throw;
        //    }
        //}

        //public void UpdateOrder(Order order)
        //{
        //    try
        //    {
        //        using (var db = new TMSContext())
        //        {
        //            db.Entry(order).State = EntityState.Modified;
        //            db.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message);
        //        throw;
        //    }
        //}

        //public void DeleteOrder(Order order)
        //{
        //    try
        //    {
        //        _orderRepository.Delete(order);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message);
        //        throw;
        //    }
        //}

        //#endregion Insert / Update / Delete
    }
}