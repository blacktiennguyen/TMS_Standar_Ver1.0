using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Core.Domains.MasterDatas;

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

        public ItemUnit GetById(int Id)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var itemUnit = db.ItemUnits
                        .Where(x => x.Id == Id)
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
    }
}