using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Core.Domains.MasterDatas;
using TMS.Library.Utils;
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

        public VehicleType GetById(int id, int companyId = 0, int tenantId = 0)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var vehicleType = db.VehicleTypes
                        .Where(x => x.Id == id && x.CompanyId == companyId && x.TenantId == tenantId)
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

        public IPagedList<VehicleType> Search(string code, string name, int pageIndex = 0, int pageSize = int.MaxValue, int companyId = 0, int tenantId = 0)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var query = db.VehicleTypes
                        .Where(x => x.CompanyId == companyId && x.TenantId == tenantId)
                        .ToList();

                    if (!String.IsNullOrEmpty(code))
                        query = query.Where(x => x.Code != null && !String.IsNullOrEmpty(x.Code) && x.Code.Contains(code.Trim()))
                                .ToList();

                    if (!String.IsNullOrEmpty(name))
                        query = query.Where(x => x.Name != null && !String.IsNullOrEmpty(x.Name) &&
                                                                                                 (Ultil.ConvertToUnSign(x.Name).IndexOf(name.Trim(), StringComparison.CurrentCultureIgnoreCase) >= 0 || x.Name.Contains(name.Trim()))
                                            )
                                .ToList();

                    query = query.OrderByDescending(x => x.CreatedDate).ToList();

                    return new PagedList<VehicleType>(query, pageIndex, pageSize);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public int SaveOrUpdate(VehicleType vehicleType)
        {
            try
            {
                if (vehicleType.Id > 0)
                {
                    using (var db = new TMSContext())
                    {
                        db.Entry(vehicleType).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    return vehicleType.Id;
                }
                else
                {
                    _vehicleTypeRepository.Insert(vehicleType);

                    return vehicleType.Id;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public void Delete(VehicleType vehicleType)
        {
            try
            {
                _vehicleTypeRepository.Delete(vehicleType);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }
    }
}