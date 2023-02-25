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
    public partial class VehicleTypeService : IVehicleTypeService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<VehicleType> _vehicleTypeRepository;

        #endregion Fields

        #region Ctor

        public VehicleTypeService(IRepository<VehicleType> vehicleTypeRepository)
        {
            this._vehicleTypeRepository = vehicleTypeRepository;
        }

        #endregion Ctor

        public VehicleType GetById(int Id)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var vehicleType = db.VehicleTypes
                        .Where(x => x.Id == Id)
                        .FirstOrDefault();

                    return vehicleType;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public List<VehicleType> GetAlls(int companyId, int tenantId)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var vehicleTypes = db.VehicleTypes
                        .Where(x => x.CompanyId == companyId && x.TenantId == tenantId)
                        .ToList();

                    return vehicleTypes;
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