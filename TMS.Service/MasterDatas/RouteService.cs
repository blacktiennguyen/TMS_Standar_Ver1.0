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
    public partial class RouteService : IRouteService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<Route> _routeRepository;

        #endregion Fields

        #region Ctor

        public RouteService(IRepository<Route> routeRepository)
        {
            this._routeRepository = routeRepository;
        }

        #endregion Ctor

        public Route GetById(int Id)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var route = db.Routes
                        .Where(x => x.Id == Id)
                        .FirstOrDefault();

                    return route;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public List<Route> GetAlls(int companyId, int tenantId)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var route = db.Routes
                        .Where(x => x.CompanyId == companyId && x.TenantId == tenantId)
                        .ToList();

                    return route;
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