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
    public partial class CurrencyService : ICurrencyService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<Currency> _currencyRepository;

        #endregion Fields

        #region Ctor

        public CurrencyService(IRepository<Currency> currencyRepository)
        {
            this._currencyRepository = currencyRepository;
        }

        #endregion Ctor

        public Currency GetLocalCurrencyByCompanyId(int companyId, int tenantId)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var company = db.Companys
                        .Where(x => x.Id == companyId && x.TenantId == tenantId)
                        .FirstOrDefault();

                    if (company != null)
                    {
                        var localCurrencyId = company.LocalCurrencyId;

                        var itemUnit = db.Currencies
                        .Where(x => x.Id == localCurrencyId && x.TenantId == tenantId)
                        .FirstOrDefault();

                        return itemUnit;
                    }
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public Currency GetById(int Id, int companyId, int tenantId)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var itemUnit = db.Currencies
                        .Where(x => x.Id == Id && x.CompanyId == companyId && x.TenantId == tenantId)
                        .FirstOrDefault();

                    return itemUnit;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public List<Currency> GetAlls(int companyId, int tenantId)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var currencies = db.Currencies
                        .Where(x => x.CompanyId == companyId && x.TenantId == tenantId)
                        .ToList();

                    return currencies;
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