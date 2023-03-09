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

        public Route GetById(int id, int companyId = 0, int tenantId = 0)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var route = db.Routes
                        .Where(x => x.Id == id && x.CompanyId == companyId && x.TenantId == tenantId)
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

        public IPagedList<Route> Search(string code, string name, int pageIndex = 0, int pageSize = int.MaxValue, int companyId = 0, int tenantId = 0)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var query = db.Routes
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

                    return new PagedList<Route>(query, pageIndex, pageSize);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public int SaveOrUpdate(Route route)
        {
            try
            {
                if (route.Id > 0)
                {
                    using (var db = new TMSContext())
                    {
                        db.Entry(route).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    return route.Id;
                }
                else
                {
                    _routeRepository.Insert(route);

                    return route.Id;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public void Delete(Route route)
        {
            try
            {
                _routeRepository.Delete(route);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }
    }
}