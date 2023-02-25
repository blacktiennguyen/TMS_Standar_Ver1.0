using System;
using System.Collections.Generic;
using System.Linq;
using TMS.Core;
using TMS.Core.Domains;

namespace TMS.Service
{
    public partial class FunctionService : IFunctionService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<Function> _functionRepository;

        #endregion Fields

        #region Ctor

        public FunctionService(IRepository<Function> functionRepository)
        {
            this._functionRepository = functionRepository;
        }

        #endregion Ctor

        public IList<Function> GetAllFuntions(int? parentID, int companyID = 0, int tenantID = 0)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var query = db.Functions.Where(x => x.ParentId == parentID && x.IsActive == true).OrderBy(x => x.DisplayOrder).ToList();

                    return query.ToList();
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