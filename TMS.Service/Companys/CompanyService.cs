using System;
using System.Collections.Generic;
using System.Linq;
using TMS.Core;
using TMS.Core.Domains;

namespace TMS.Service.Companys
{
    public partial class CompanyService : ICompanyService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<Company> _companyRepository;

        #endregion Fields

        #region Ctor

        public CompanyService(IRepository<Company> companyRepository)
        {
            this._companyRepository = companyRepository;
        }

        #endregion Ctor

        public IList<Company> GetAll(int tenantId)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var query = db.Companys.Where(x => x.TenantId == tenantId && x.IsActive)
                        .ToList();

                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public Company GetById(int Id, int tenantId)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var company = db.Companys.Where(x => x.Id == Id && x.TenantId == tenantId && x.IsActive)
                        .FirstOrDefault();

                    return company;
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