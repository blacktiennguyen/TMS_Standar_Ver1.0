using System;
using System.Linq;
using TMS.Core;
using TMS.Core.Domains;
using TMS.Core.Domains.FIXs;

namespace TMS.Service.FIXs
{
    public partial class OrderStatusService : IOrderStatusService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<OrderStatus> _orderStatusRepository;

        #endregion Fields

        #region Ctor

        public OrderStatusService(IRepository<OrderStatus> orderStatusRepository)
        {
            this._orderStatusRepository = orderStatusRepository;
        }

        #endregion Ctor

        public OrderStatus GetById(int Id)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var orderStatus = db.OrderStatus
                        .Where(x => x.Id == Id)
                        .FirstOrDefault();

                    return orderStatus;
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