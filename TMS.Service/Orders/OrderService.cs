using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Core.Domains;
using TMS.Core.Extend;

namespace TMS.Service.Orders
{
    public class OrderService : IOrderService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<Order> _orderRepository;

        #endregion Fields

        #region Ctor

        public OrderService(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        #endregion Ctor

        public Order GetById(int Id, int companyId, int tenantId)
        {
            var order = new Order();
            try
            {
                using (var db = new TMSContext())
                {
                    order = db.Orders
                        .Where(x => x.Id == Id && x.CompanyId == companyId && x.TenantId == tenantId)
                        .FirstOrDefault();

                    return order;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public Order GetByOrderCode(string orderCode, int companyId, int tenantId)
        {
            var order = new Order();
            try
            {
                using (var db = new TMSContext())
                {
                    order = db.Orders
                        .Where(x => x.OrderCode == orderCode && x.CompanyId == companyId && x.TenantId == tenantId)
                        .FirstOrDefault();

                    return order;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public bool CheckExistData(int id = 0, string code = "", int companyId = 0, int tenantId = 0)
        {
            var isExist = false;
            try
            {
                using (var db = new TMSContext())
                {
                    isExist = db.Orders
                        .Where(x => x.Id != id && x.OrderCode == code && x.CompanyId == companyId && x.TenantId == tenantId)
                        .Any();

                    return isExist;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return false;
            }
        }

        public IPagedList<Order> SearchOrder(OrderSearchExtend orderSearchExtend, int pageIndex = 0, int pageSize = int.MaxValue, int companyId = 0, int tenantId = 0)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var query = db.Orders
                        .Where(x => x.CompanyId == companyId && x.TenantId == tenantId)
                        .ToList();

                    if (!String.IsNullOrEmpty(orderSearchExtend.OrderCode))
                        query = query.Where(x => x.OrderCode != null && !String.IsNullOrEmpty(x.OrderCode) &&
                                                 x.OrderCode.Contains(orderSearchExtend.OrderCode))
                                .ToList();

                    if (!String.IsNullOrEmpty(orderSearchExtend.BillOfLadingCode))
                        query = query.Where(x => x.BillOfLadingCode != null && !String.IsNullOrEmpty(x.BillOfLadingCode) &&
                                                 x.BillOfLadingCode.Contains(orderSearchExtend.BillOfLadingCode))
                                     .ToList();

                    //if (orderSearchExtend.SearchExpectedDeliveryDateFrom != null && orderSearchExtend.SearchExpectedDeliveryDateFrom != DateTime.MinValue)
                    //    query = query.Where(x => x.ExpectedDeliveryDate >= orderSearchExtend.SearchExpectedDeliveryDateFrom)
                    //                 .ToList();

                    //if (orderSearchExtend.SearchExpectedDeliveryDateTo != null && orderSearchExtend.SearchExpectedDeliveryDateTo != DateTime.MinValue)
                    //    query = query.Where(x => x.ExpectedDeliveryDate <= orderSearchExtend.SearchExpectedDeliveryDateTo)
                    //                 .ToList();

                    //if (orderSearchExtend.SearchEstimatedDeliveryStartDateFrom != null && orderSearchExtend.SearchEstimatedDeliveryStartDateFrom != DateTime.MinValue)
                    //    query = query.Where(x => x.EstimatedDeliveryStartDate >= orderSearchExtend.SearchEstimatedDeliveryStartDateFrom)
                    //                 .ToList();

                    //if (orderSearchExtend.SearchEstimatedDeliveryStartDateTo != null && orderSearchExtend.SearchEstimatedDeliveryStartDateTo != DateTime.MinValue)
                    //    query = query.Where(x => x.EstimatedDeliveryStartDate <= orderSearchExtend.SearchEstimatedDeliveryStartDateTo)
                    //                 .ToList();

                    //if (orderSearchExtend.SearchEstimatedDeliveryEndDateFrom != null && orderSearchExtend.SearchEstimatedDeliveryEndDateFrom != DateTime.MinValue)
                    //    query = query.Where(x => x.EstimatedDeliveryEndDate >= orderSearchExtend.SearchEstimatedDeliveryEndDateFrom)
                    //                 .ToList();

                    //if (orderSearchExtend.SearchEstimatedDeliveryEndDateFrom != null && orderSearchExtend.SearchEstimatedDeliveryEndDateFrom != DateTime.MinValue)
                    //    query = query.Where(x => x.EstimatedDeliveryEndDate <= orderSearchExtend.SearchEstimatedDeliveryEndDateFrom)
                    //                 .ToList();

                    //if (orderSearchExtend.VehicleTypeId != null && orderSearchExtend.VehicleTypeId > 0)
                    //    query = query.Where(x => x.VehicleTypeId == orderSearchExtend.VehicleTypeId)
                    //                 .ToList();

                    //if (orderSearchExtend.TransporterId != null && orderSearchExtend.TransporterId > 0)
                    //    query = query.Where(x => x.TransporterId == orderSearchExtend.TransporterId)
                    //                 .ToList();

                    #region Address From - To

                    //if (orderSearchExtend.CountryFromId != null && orderSearchExtend.CountryFromId > 0)
                    //    query = query.Where(x => x.CountryFromId == orderSearchExtend.CountryFromId)
                    //                 .ToList();

                    //if (orderSearchExtend.ProvinceFromId != null && orderSearchExtend.ProvinceFromId > 0)
                    //    query = query.Where(x => x.ProvinceFromId == orderSearchExtend.ProvinceFromId)
                    //                 .ToList();

                    //if (orderSearchExtend.DistrictFromId != null && orderSearchExtend.DistrictFromId > 0)
                    //    query = query.Where(x => x.DistrictFromId == orderSearchExtend.DistrictFromId)
                    //                 .ToList();

                    //if (orderSearchExtend.WardFromId != null && orderSearchExtend.WardFromId > 0)
                    //    query = query.Where(x => x.WardFromId == orderSearchExtend.WardFromId)
                    //                 .ToList();

                    //if (orderSearchExtend.CountryToId != null && orderSearchExtend.CountryToId > 0)
                    //    query = query.Where(x => x.CountryToId == orderSearchExtend.CountryToId)
                    //                 .ToList();

                    //if (orderSearchExtend.ProvinceToId != null && orderSearchExtend.ProvinceToId > 0)
                    //    query = query.Where(x => x.ProvinceToId == orderSearchExtend.ProvinceToId)
                    //                 .ToList();

                    //if (orderSearchExtend.DistrictToId != null && orderSearchExtend.DistrictToId > 0)
                    //    query = query.Where(x => x.DistrictToId == orderSearchExtend.DistrictToId)
                    //                 .ToList();

                    //if (orderSearchExtend.WardToId != null && orderSearchExtend.WardToId > 0)
                    //    query = query.Where(x => x.WardToId == orderSearchExtend.WardToId)
                    //                 .ToList();

                    #endregion Address From - To

                    query = query.OrderByDescending(x => x.OrderCreateDate).ToList();

                    return new PagedList<Order>(query, pageIndex, pageSize);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        #region Insert / Update / Delete

        public void InsertOrder(Order order)
        {
            try
            {
                _orderRepository.Insert(order);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public void UpdateOrder(Order order)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public void DeleteOrder(Order order)
        {
            try
            {
                _orderRepository.Delete(order);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        #endregion Insert / Update / Delete
    }
}