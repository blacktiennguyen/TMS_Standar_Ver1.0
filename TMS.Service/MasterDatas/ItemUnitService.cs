using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Core.Domains.MasterDatas;
using TMS.Library.Utils;

namespace TMS.Service.MasterDatas
{
    public partial class ItemUnitService : IItemUnitService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<ItemUnit> _itemUnitRepository;

        #endregion Fields

        #region Ctor

        public ItemUnitService(IRepository<ItemUnit> itemUnitRepository)
        {
            this._itemUnitRepository = itemUnitRepository;
        }

        #endregion Ctor

        public ItemUnit GetById(int id, int companyId = 0, int tenantId = 0)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var ItemUnit = db.ItemUnits
                        .Where(x => x.Id == id && x.CompanyId == companyId && x.TenantId == tenantId)
                        .FirstOrDefault();

                    return ItemUnit;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public IPagedList<ItemUnit> Search(string code, string name, int pageIndex = 0, int pageSize = int.MaxValue, int companyId = 0, int tenantId = 0)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var query = db.ItemUnits
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

                    return new PagedList<ItemUnit>(query, pageIndex, pageSize);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public int SaveOrUpdate(ItemUnit itemUnit)
        {
            try
            {
                if (itemUnit.Id > 0)
                {
                    using (var db = new TMSContext())
                    {
                        db.Entry(itemUnit).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    return itemUnit.Id;
                }
                else
                {
                    _itemUnitRepository.Insert(itemUnit);

                    return itemUnit.Id;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public void Delete(ItemUnit ItemUnit)
        {
            try
            {
                _itemUnitRepository.Delete(ItemUnit);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }
    }
}