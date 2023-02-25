using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Core.Domains.MasterDatas;

namespace TMS.Service.MasterDatas
{
    public partial class CountryService : ICountryService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<Country> _countryRepository;

        #endregion Fields

        #region Ctor

        public CountryService(IRepository<Country> countryRepository)
        {
            this._countryRepository = countryRepository;
        }

        #endregion Ctor

        public Country GetById(int Id)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var country = db.Countrys
                        .Where(x => x.Id == Id)
                        .FirstOrDefault();

                    return country;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public List<Country> GetAlls()
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var countries = db.Countrys
                        .OrderByDescending(x => x.IsDefault == true)
                        .ToList();

                    return countries;
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