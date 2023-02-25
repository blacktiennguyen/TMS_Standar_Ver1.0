using System;
using System.Linq;
using TMS.Core;
using TMS.Core.Domains;
using TMS.Core.Domains.FIXs;

namespace TMS.Service.FIXs
{
    public partial class OrderShippingStatusService : IOrderShippingStatusService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<OrderShippingStatus> _orderShippingStatusRepository;

        #endregion Fields

        #region Ctor

        public OrderShippingStatusService(IRepository<OrderShippingStatus> orderShippingStatusRepository)
        {
            this._orderShippingStatusRepository = orderShippingStatusRepository;
        }

        #endregion Ctor

        public OrderShippingStatus GetById(int Id)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var orderShippingStatus = db.OrderShippingStatus
                        .Where(x => x.Id == Id)
                        .FirstOrDefault();

                    return orderShippingStatus;
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