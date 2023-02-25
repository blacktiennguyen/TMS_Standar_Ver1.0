using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Core.Domains.MasterDatas;
using TMS.Shared.Const;

namespace TMS.Service.MasterDatas
{
    public partial class TransporterService : ITransporterService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<Transporter> _transporterRepository;

        #endregion Fields

        #region Ctor

        public TransporterService(IRepository<Transporter> transporterRepository)
        {
            this._transporterRepository = transporterRepository;
        }

        #endregion Ctor

        public Transporter GetById(int Id)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var transporter = db.Transporters
                        .Where(x => x.Id == Id)
                        .FirstOrDefault();

                    return transporter;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public List<Transporter> GetAlls(int companyId, int tenantId)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var transporters = db.Transporters
                        .Where(x => x.CompanyId == companyId && x.TenantId == tenantId)
                        .ToList();

                    return transporters;
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