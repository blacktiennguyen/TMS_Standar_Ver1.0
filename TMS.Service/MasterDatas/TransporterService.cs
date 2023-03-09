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
    public partial class TransporterService : ITransporterService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<Transporter> _transporterRepository;

        #endregion Fields

        #region Ctor

        public TransporterService(IRepository<Transporter> transporterRepository)
        {
            this._transporterRepository = transporterRepository;
        }

        #endregion Ctor

        public List<Transporter> GetAlls(int companyId, int tenantId)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var transporters = db.Transporters
                        .Where(x => x.CompanyId == companyId && x.TenantId == tenantId)
                        .ToList();

                    return transporters;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public Transporter GetById(int id, int companyId = 0, int tenantId = 0)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var transporter = db.Transporters
                        .Where(x => x.Id == id && x.CompanyId == companyId && x.TenantId == tenantId)
                        .FirstOrDefault();

                    return transporter;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public IPagedList<Transporter> Search(string code, string name, string phone, int pageIndex = 0, int pageSize = int.MaxValue, int companyId = 0, int tenantId = 0)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var query = db.Transporters
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

                    if (!String.IsNullOrEmpty(phone))
                        query = query.Where(x => x.Phone != null && !String.IsNullOrEmpty(x.Phone) && x.Phone.Contains(phone.Trim()))
                                .ToList();

                    query = query.OrderByDescending(x => x.CreatedDate).ToList();

                    return new PagedList<Transporter>(query, pageIndex, pageSize);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public int SaveOrUpdate(Transporter transporter)
        {
            try
            {
                if (transporter.Id > 0)
                {
                    using (var db = new TMSContext())
                    {
                        db.Entry(transporter).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    return transporter.Id;
                }
                else
                {
                    _transporterRepository.Insert(transporter);

                    return transporter.Id;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public void Delete(Transporter transporter)
        {
            try
            {
                _transporterRepository.Delete(transporter);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }
    }
}