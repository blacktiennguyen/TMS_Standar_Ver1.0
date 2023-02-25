using System;
using System.Collections.Generic;
using System.Linq;
using TMS.Core;
using TMS.Core.Domains.FIXs;

namespace TMS.Service.FIXs
{
    public partial class PayerPostageServiceService : IPayerPostageServiceService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<PayerPostageService> _payerPostageServiceRepository;

        #endregion Fields

        #region Ctor

        public PayerPostageServiceService(IRepository<PayerPostageService> payerPostageServiceRepository)
        {
            this._payerPostageServiceRepository = payerPostageServiceRepository;
        }

        #endregion Ctor

        public List<PayerPostageService> GetAlls()
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var payerPostageServices = db.PayerPostageServices
                        .ToList();

                    return payerPostageServices;
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