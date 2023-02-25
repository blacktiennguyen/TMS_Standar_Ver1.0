using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Core.Domains.MasterDatas;

namespace TMS.Service.MasterDatas
{
    public partial class ProvinceService : IProvinceService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<Province> _provinceRepository;

        #endregion Fields

        #region Ctor

        public ProvinceService(IRepository<Province> provinceRepository)
        {
            this._provinceRepository = provinceRepository;
        }

        #endregion Ctor

        public Province GetById(int Id)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var province = db.Provinces
                        .Where(x => x.Id == Id)
                        .FirstOrDefault();

                    return province;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public List<Province> GetAllsByCountryId(int countryId)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    if (countryId == 0)
                    {
                        var getCountryDefaultId = db.Countrys.Where(x => x.IsDefault).FirstOrDefault();

                        countryId = getCountryDefaultId?.Id ?? 0;
                    }
                    var provinces = db.Provinces
                        .Where(x => x.CountryId == countryId)
                        .ToList();

                    return provinces;
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