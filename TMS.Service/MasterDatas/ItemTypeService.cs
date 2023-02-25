using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Core.Domains.MasterDatas;

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

        public ItemType GetById(int Id)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var itemType = db.ItemTypes
                        .Where(x => x.Id == Id)
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
    }
}