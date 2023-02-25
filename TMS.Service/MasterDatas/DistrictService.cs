using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Core.Domains.MasterDatas;

namespace TMS.Service.MasterDatas
{
    public partial class DistrictService : IDistrictService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<District> _districtRepository;

        #endregion Fields

        #region Ctor

        public DistrictService(IRepository<District> districtRepository)
        {
            this._districtRepository = districtRepository;
        }

        #endregion Ctor

        public District GetById(int Id)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var district = db.Districts
                        .Where(x => x.Id == Id)
                        .FirstOrDefault();

                    return district;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public List<District> GetAllsByProvinceId(int provinceId)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var districts = db.Districts
                        .Where(x => x.ProvinceId == provinceId)
                        .ToList();

                    return districts;
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