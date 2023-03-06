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
    public partial class ItemTypeService : IItemTypeService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<ItemType> _itemTypeRepository;

        #endregion Fields

        #region Ctor

        public ItemTypeService(IRepository<ItemType> itemTypeRepository)
        {
            this._itemTypeRepository = itemTypeRepository;
        }

        #endregion Ctor

        public ItemType GetById(int id, int companyId = 0, int tenantId = 0)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var itemType = db.ItemTypes
                        .Where(x => x.Id == id && x.CompanyId == companyId && x.TenantId == tenantId)
                        .FirstOrDefault();

                    return itemType;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public IPagedList<ItemType> Search(string code, string name, int pageIndex = 0, int pageSize = int.MaxValue, int companyId = 0, int tenantId = 0)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var query = db.ItemTypes
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

                    return new PagedList<ItemType>(query, pageIndex, pageSize);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public int SaveOrUpdate(ItemType itemType)
        {
            try
            {
                if (itemType.Id > 0)
                {
                    using (var db = new TMSContext())
                    {
                        db.Entry(itemType).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    return itemType.Id;
                }
                else
                {
                    _itemTypeRepository.Insert(itemType);

                    return itemType.Id;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public void Delete(ItemType itemType)
        {
            try
            {
                _itemTypeRepository.Delete(itemType);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }
    }
}