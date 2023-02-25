using System;
using System.Collections.Generic;
using System.Linq;
using TMS.Core;
using TMS.Core.Domains.FIXs;

namespace TMS.Service.FIXs
{
    public partial class TransportationMethodService : ITransportationMethodService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<TransportationMethod> _transportationMethodRepository;

        #endregion Fields

        #region Ctor

        public TransportationMethodService(IRepository<TransportationMethod> transportationMethodRepository)
        {
            this._transportationMethodRepository = transportationMethodRepository;
        }

        #endregion Ctor

        public List<TransportationMethod> GetAlls()
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var transportationMethods = db.TransportationMethods
                        .ToList();

                    return transportationMethods;
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