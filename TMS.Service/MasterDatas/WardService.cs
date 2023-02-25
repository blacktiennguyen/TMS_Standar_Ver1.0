using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Core.Domains.MasterDatas;

namespace TMS.Service.MasterDatas
{
    public partial class WardService : IWardService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<Ward> _wardRepository;

        #endregion Fields

        #region Ctor

        public WardService(IRepository<Ward> wardRepository)
        {
            this._wardRepository = wardRepository;
        }

        #endregion Ctor

        public Ward GetById(int Id)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var ward = db.Wards
                        .Where(x => x.Id == Id)
                        .FirstOrDefault();

                    return ward;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public List<Ward> GetAllsByDistrictId(int districtId)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var wards = db.Wards
                        .Where(x => x.DistrictId == districtId)
                        .ToList();

                    return wards;
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